using Lururen.Core.CommandSystem;
using Lururen.Core.Networking;
using Lururen.Networking.Common.Commands;
using Lururen.Networking.Common.Messages;
using System.Net;
using System.Net.Sockets;

namespace Lururen.Networking.SocketNetworking
{
    public class SocketServerMessageBridge : IServerMessageBridge
    {
        #region ServerMessageBridge

        public event OnCommandEventHandler? OnCommand;

        public async Task SendContiniousData(Guid clientGuid, Stream resourceStream)
        {
            await Clients[clientGuid].SendContiniousData(resourceStream);
        }

        public async Task SendData(Guid clientGuid, object data)
        {
            _ = await Clients[clientGuid].Send(data);
        }

        public async Task Start()
        {
            CancellationTokenSource = new();
            IPEndPoint ipPoint = new(IPAddress.Any, Port);
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);

            while (!CancellationTokenSource.IsCancellationRequested)
            {
                Socket handler = await socket.AcceptAsync(CancellationTokenSource.Token);
                _ = StartSession(handler);
            }
        }

        public async Task Stop()
        {
            foreach (KeyValuePair<Guid, Socket> entry in Clients)
            {
                _ = await entry.Value.Send(new ServerStatus(ServerState.CLOSED));
            }
            _ = Parallel.ForEach(Clients, x => x.Value.Close());
            Clients.Clear();
        }

        public virtual void Dispose()
        {
        }

        #endregion ServerMessageBridge

        public SocketServerMessageBridge(int port = 7777)
        {
            Port = port;
        }

        public int Port { get; private set; }

        protected Dictionary<Guid, Socket> Clients { get; set; } = new();

        private CancellationTokenSource? CancellationTokenSource { get; set; }

        public List<Guid> GetClients()
        {
            return Clients.Keys.ToList();
        }

        /// <summary>
        /// Starts connection with client and invokes all incoming commands.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private async Task StartSession(Socket socket)
        {
            if (CancellationTokenSource is null)
            {
                // Error, Bridge was not started
                return;
            }
            Guid guid = Guid.NewGuid();
            Clients.Add(guid, socket);
            ICommand? command = null;
            try
            {
                while (command is not DisconnectCommand)
                {
                    command = await socket.Recieve<ICommand>(CancellationTokenSource.Token);
                    OnCommand?.Invoke(guid, command);
                }
                _ = Clients.Remove(guid);
                socket.Close();
            }
            catch (Exception)
            {
                // Add logging
                _ = Clients.Remove(guid);
                socket.Close();
            }
        }
    }
}