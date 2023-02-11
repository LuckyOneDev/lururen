using System.Reflection;
using Lururen.Net.Common.Controller.Attributes;
using Lururen.Net.Common.Packets;

namespace Lururen.Net.Common.Controller
{
    /// <summary>
    /// Base class for all controllers.
    /// Controllers are used to handle requests and send them using abstract stream.
    /// Does not own the stream, but uses it to read data.
    /// </summary>
    public class ControllerBase<TController>
    {
        private static readonly Dictionary<PacketType, Func<Packet, Packet>> _packetTypeToMethod = new();
        public Stream PacketStream { get; set; }
        public CancellationTokenSource? CancellationTokenSource { get; set; }

        static ControllerBase()
        {
            foreach (MethodInfo method in typeof(TController).GetMethods())
            {
                IEnumerable<ResponseToAttribute> attributes = method.GetCustomAttributes<ResponseToAttribute>();
                foreach (ResponseToAttribute attribute in attributes)
                {
                    _packetTypeToMethod.Add(attribute.PacketType,
                        (Func<Packet, Packet>)Delegate.CreateDelegate(typeof(Func<Packet, Packet>), method));
                }
            }
        }
        public ControllerBase(Stream packetStream)
        {
            PacketStream = packetStream;
        }

        public void HandlePacket(Packet packet)
        {
            if (_packetTypeToMethod.TryGetValue(packet.PacketType, out Func<Packet, Packet>? handler))
            {
                if (handler(packet) is Packet p)
                {
                    SendPacketAsync(p);
                }
            }
        }

        public async void SendPacketAsync(Packet packet)
        {
            await PacketStream.WriteAsync(packet.ToBytes());
        }


        public void StopRecievingPackets()
        {
            CancellationTokenSource?.Cancel();
        }

        public Task StartRecievingPackets(CancellationTokenSource cts)
        {
            CancellationTokenSource = cts;
            CancellationToken token = cts.Token;
            // The task would stop when the token is cancelled or the stream is closed.
            return Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        Packet? packet = await Packet.FromStream(PacketStream);
                        if (packet is Packet p)
                        {
                            HandlePacket(p);
                        }
                    }
                    // If error occurs, stop waiting for data.
                    catch (IOException e)
                    {
                        // Ignore, but log, to make sure it was not closed by real error.
                        Console.Error.WriteLine(e);
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        // Ignore - stream was closed by client/server.
                        break;
                    }
                }
            }, token);
        }

    }
}