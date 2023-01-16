using Lururen.Core.CommandSystem;
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

        public event OnDataEventHandler OnData;

        public async Task Start()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(Host, Port);
            this.Socket = socket;
            _ = SocketHelper.Recieve<object>(Socket).ContinueWith(x => {
                OnData.Invoke(x);
            });
        }

        public async Task Stop()
        {
            if (Socket.Poll(1000, SelectMode.SelectWrite))
            {
                await SocketHelper.Send(Socket, new DisconnectCommand());
                await Socket.DisconnectAsync(true);
            } else
            {
                Socket.Close();
            }

        }

        public void Dispose()
        {
            this.Socket?.Dispose();
        }

        public async Task SendCommand(ICommand command)
        {
            if (this.Socket is not null && Socket.Poll(1000, SelectMode.SelectWrite))
            {
                await SocketHelper.Send(this.Socket, command);
            }
            else
            {
                throw new Exception("Socket was not initialized");
            }
        }
    }
}