using Lururen.Core.EntitySystem;
using Lururen.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lururen.Networking.SimpleSocketBus
{
    public abstract class SocketDataBus : IDataBus
    {
        public SocketDataBus(int port = 7777)
        {
            this.Port = port;
        }
        public bool Running { get; protected set; }
        public int Port { get; private set; }

        public async Task Start()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, Port);
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);

            Running = true;
            while (Running)
            {
                var handler = await socket.AcceptAsync();
                _ = StartSession(handler);
            }
        }

        async Task StartSession(Socket handler)
        {
            IMessage message = null;
            try
            {
                while (message is not DisconnectMessage)
                {
                    message = await SocketHelper.Recieve<IMessage>(handler);
                    var response = await OnMessage(message);
                    await SocketHelper.Send(handler, response);
                }
                handler.Close();
            } catch (Exception ex)
            {
                handler.Close();
            }
        }
         
        public async Task Stop()
        {
            Running = false;
        }

        public abstract Task<IEnumerable<Entity>> OnMessage(IMessage command);

        public void Dispose()
        {
        }
    }
}
