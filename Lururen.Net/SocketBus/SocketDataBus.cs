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

namespace Lururen.Networking.SocketBus
{
    public abstract class SocketDataBus : IDataBus
    {
        public bool Working { get; private set; }

        public async Task Start(int port=7777)
        {
            Working = true;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, port);
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);
            while (Working)
            {
                var handler = await socket.AcceptAsync();
                string data = "";
                var bytes = new byte[1024];

                int bytesRec = handler.Receive(bytes, 1024, 0);
                data += Encoding.Unicode.GetString(bytes, 0, bytesRec);

                while (handler.Available > 0)
                {
                    bytesRec = handler.Receive(bytes, 1024, 0);
                    data += Encoding.Unicode.GetString(bytes, 0, bytesRec);   
                }

                var message = JsonConvert.DeserializeObject(data, 
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                var response = await OnMessage((IMessage)message);
                var jsonResponse = JsonConvert.SerializeObject(response,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                int sendResult = await handler.SendAsync(Encoding.Unicode.GetBytes(jsonResponse));
                handler.Dispose();
            }
            
        }

        public void Stop()
        {
            Working = false;
        }

        public abstract Task<IEnumerable<Entity>> OnMessage(IMessage command);
    }
}
