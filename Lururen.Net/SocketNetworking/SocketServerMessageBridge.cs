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
            await SocketHelper.SendContiniousData(Clients[clientGuid], resourceStream);
        }

        public async Task SendData(Guid clientGuid, object data)
        {
            await SocketHelper.Send(Clients[clientGuid], data);
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
                var handler = await socket.AcceptAsync(CancellationTokenSource.Token);
                _ = StartSession(handler);
            }
        }

        public async Task Stop()
        {
            foreach (var entry in Clients)
            {
                await SocketHelper.Send(entry.Value, new ServerStatus(ServerState.CLOSED));
            }
            Parallel.ForEach(Clients, x => x.Value.Close());
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
            Guid guid = Guid.NewGuid();
            Clients.Add(guid, socket);
            ICommand? command = null;
            try
            {
                while (command is not DisconnectCommand)
                {
                    command = await SocketHelper.Recieve<ICommand>(socket, CancellationTokenSource.Token);
                    OnCommand?.Invoke(guid, command);
                }
                Clients.Remove(guid);
                socket.Close();
            }
            catch (Exception ex)
            {
                // Add logging
                Clients.Remove(guid);
                socket.Close();
            }
        }
    }
}