using System.Net.Sockets;
using Lururen.Common.CommandSystem;
using Lururen.Common.Networking.SocketNetworking;
using Lururen.Net.Client.Controllers;

namespace Lururen.Net.Client.SocketNetworking
{
    public class SocketClient : ProtocolMessageBridge
    {
        public SocketClientMessageBridge(string host = "127.0.0.1", int port = 7777) : base("ClientData")
        {
            Host = host;
            Port = port;
        }

        #region IClientMessageBridge

        public override void Dispose()
        {
            Socket?.Dispose();
        }

        public override async Task SendCommand(ICommand command)
        {
            if (Socket is not null)
            {
                _ = await Socket.Send(command);
            }
            else
            {
                throw new Exception("Socket was not initialized");
            }
        }

        public override async Task Start()
        {
            CancellationTokenSource = new();
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(Host, Port);
            Socket = socket;
            _ = StartRecieveData();
        }

        public override async Task Stop()
        {
            CancellationTokenSource?.Cancel();
            if (Socket is not null)
            {
                _ = await Socket.Send(new DisconnectCommandDTO());
                await Socket.DisconnectAsync(true);
            }
        }

        #endregion INetBus

        public string Host { get; protected set; }
        public int Port { get; protected set; }
        private CancellationTokenSource? CancellationTokenSource { get; set; }
        private Socket? Socket { get; set; }

        private async Task StartRecieveData()
        {
            if (CancellationTokenSource is null
                || Socket is null)
            {
                // Error: Bridge was not started
                return;
            }
            CancellationToken token = CancellationTokenSource.Token;
            while (!token.IsCancellationRequested)
            {
                switch (ProtocolMessagingMode)
                {
                    case ProtocolMessagingMode.Default:
                        await Socket.Recieve<object>(token).ContinueWith((task) =>
                        {
                            object data = task.Result;
                            ProcessMessage(data);
                        }, token);
                        break;

                    case ProtocolMessagingMode.Stream:
                        await Socket.RecieveBytes(token).ContinueWith((task) =>
                        {
                            ArraySegment<byte> data = task.Result;
                            ProcessMessage(data);
                        }, token);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}