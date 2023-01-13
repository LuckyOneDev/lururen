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

namespace Lururen.Networking.SocketBus
{
    public class SocketNetBus : INetBus
    {
        public Socket? Socket { get; private set; }

        public async Task Connect(string host="127.0.0.1", int port=7777)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(host, port);
            this.Socket = socket;
        }

        public async Task<IEnumerable<Entity>> SendMessage(IMessage message)
        {
            if (this.Socket is not null)
            {
                var json = JsonConvert.SerializeObject(message, message.GetType(),
                                        new JsonSerializerSettings()
                                        {
                                            TypeNameHandling = TypeNameHandling.All
                                        });
                await Socket.SendAsync(Encoding.Unicode.GetBytes(json));

                string data = "";
                var bytes = new byte[1024];
                int bytesRec = Socket.Receive(bytes, 1024, 0);
                data += Encoding.Unicode.GetString(bytes, 0, bytesRec);
                while (Socket.Available > 0)
                {
                    bytesRec = Socket.Receive(bytes, 1024, 0);
                    data += Encoding.Unicode.GetString(bytes, 0, bytesRec);
                }
                return JsonConvert.DeserializeObject<IEnumerable<Entity>>(data,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
            } else
            {
                throw new Exception("Socket was not initialized");
            }
        }
    }
}
