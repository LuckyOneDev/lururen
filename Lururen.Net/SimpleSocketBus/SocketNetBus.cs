using Lururen.Core.CommandSystem;
using Lururen.Networking.Common;
using Lururen.Networking.Common.Commands;
using System.Net.Sockets;

namespace Lururen.Networking.SimpleSocketBus
{
    public class SocketNetBus : ProtocolNetBus
    {
        public SocketNetBus(string host = "127.0.0.1", int port = 7777) : base("ClientData")
        {
            this.Host = host;
            this.Port = port;
        }

        #region INetBus
        public override void Dispose()
        {
            this.Socket?.Dispose();
        }

        public override async Task SendCommand(ICommand command)
        {
            if (this.Socket is not null)
            {
                await SocketHelper.Send(this.Socket, command);
            }
            else
            {
                throw new Exception("Socket was not initialized");
            }
        }

        public override async Task Start()
        {
            CancellationTokenSource = new();
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(Host, Port);
            this.Socket = socket;
            _ = StartRecieveData();
        }

        public override async Task Stop()
        {
            CancellationTokenSource.Cancel();
            await SocketHelper.Send(Socket, new DisconnectCommand());
            await Socket.DisconnectAsync(true);
        }
        #endregion INetBus

        public string Host { get; protected set; }
        public int Port { get; protected set; }
        private CancellationTokenSource CancellationTokenSource { get; set; }
        private Socket? Socket { get; set; }
        private async Task StartRecieveData()
        {
            while (!CancellationTokenSource.Token.IsCancellationRequested)
            {
                if (ContiniousTransmissionHandler == null)
                {
                    await SocketHelper.Recieve<object>(Socket, CancellationTokenSource.Token).ContinueWith((task) =>
                    {
                        var data = task.Result;
                        ProcessSystemRequest(data);
                    }, CancellationTokenSource.Token);
                } else
                {
                    await SocketHelper.RecieveBytes(Socket, CancellationTokenSource.Token).ContinueWith((task) =>
                    {
                        var data = task.Result;
                        ProcessSystemRequest(data);
                    }, CancellationTokenSource.Token);
                }  
            }
        }
    }
}