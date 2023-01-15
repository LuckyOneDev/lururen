using Lururen.Core.EntitySystem;
using Lururen.Networking.Common;
using System.Net.Sockets;

namespace Lururen.Networking.SimpleSocketBus
{
    public class SocketNetBus : INetBus
    {
        public SocketNetBus(string host = "127.0.0.1", int port = 7777)
        {
            this.Host = host;
            this.Port = port;
        }

        private Socket? Socket { get; set; }
        public string Host { get; protected set; }
        public int Port { get; protected set; }

        public async Task Start()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(Host, Port);
            this.Socket = socket;
        }

        public async Task Stop()
        {
            if (Socket.Poll(1000, SelectMode.SelectWrite))
            {
                await SocketHelper.Send(Socket, new DisconnectMessage());
                await Socket.DisconnectAsync(true);
            } else
            {
                Socket.Close();
            }

        }

        public async Task<IEnumerable<Entity>> SendMessage(IMessage message)
        {
            if (this.Socket is not null && Socket.Poll(1000, SelectMode.SelectWrite))
            {
                await SocketHelper.Send(Socket, message);
                return await SocketHelper.Recieve<IEnumerable<Entity>>(Socket);
            }
            else
            {
                throw new Exception("Socket was not initialized");
            }
        }

        public void Dispose()
        {
            this.Socket?.Dispose();
        }
    }
}