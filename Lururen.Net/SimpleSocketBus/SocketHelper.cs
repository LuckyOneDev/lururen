using Lururen.Networking.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lururen.Networking.SimpleSocketBus
{
    public static class SocketHelper
    {
        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static Encoding Encoding = Encoding.Unicode;

        public static byte[] Encode(object Object)
        {
            var jsonResponse = JsonConvert.SerializeObject(Object, JsonSettings);
            return Encoding.GetBytes(jsonResponse);
        }

        public static T Decode<T>(byte[] binaryObject)
        {
            var stringObject = Encoding.GetString(binaryObject).TrimEnd('\0');
            return JsonConvert.DeserializeObject<T>(stringObject, JsonSettings);
        }

        public static async Task<T> Recieve<T>(Socket handler, int dataWidth = 1024)
        {
            List<byte[]> data = new();
            var buffer = new byte[dataWidth];

            await handler.ReceiveAsync(buffer, SocketFlags.None);
            data.Add(buffer);

            while (handler.Available > 0)
            {
                int recieved = handler.Receive(buffer, dataWidth, 0);
                data.Add(buffer);
            }
            var joinedData = data.SelectMany(i => i).ToArray();

            if (joinedData.Length == 0) throw new Exception("Null socket data");
            return Decode<T>(joinedData);
        }

        public static async Task<int> Send(Socket handler, object Object)
        {
            var encoded = Encode(Object);
            return await handler.SendAsync(encoded);
        }
    }
}
