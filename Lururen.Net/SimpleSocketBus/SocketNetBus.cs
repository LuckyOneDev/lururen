using Lururen.Core.EntitySystem;
using Lururen.Networking.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lururen.Networking.SimpleSocketBus
{
    public class SocketNetBus : INetBus
    {
        public SocketNetBus(string host = "127.0.0.1", int port = 7777)
        {
            this.Host = host;
            this.Port = port;
        }

        public Socket Socket { get; private set; }
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
            await SocketHelper.Send(Socket, new DisconnectMessage());
            await Socket.DisconnectAsync(true);
        }

        public async Task<IEnumerable<Entity>> SendMessage(IMessage message)
        {
            if (this.Socket is not null)
            {
                await SocketHelper.Send(Socket, message);
                return await SocketHelper.Recieve<IEnumerable<Entity>>(Socket);
            } else
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
