using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace Lururen.Networking.SocketNetworking
{
    public static class SocketHelper
    {
        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static Encoding Encoding = Encoding.Unicode;

        private static byte[] Encode(object Object)
        {
            var jsonResponse = JsonConvert.SerializeObject(Object, JsonSettings);
            return Encoding.GetBytes(jsonResponse);
        }

        private static T Decode<T>(ArraySegment<byte> binaryObject) where T : class
        {
            var stringObject = Encoding.GetString(binaryObject);
            return JsonConvert.DeserializeObject<T>(stringObject, JsonSettings);
        }

        public static async Task<T> Recieve<T>(Socket handler,
                                               CancellationToken token = default,
                                               int channelWidth = 4096) where T : class

        {
            var bytes = await RecieveBytes(handler, token, channelWidth);
            return Decode<T>(bytes);
        }

        public static async Task<ArraySegment<byte>> RecieveBytes(Socket handler,
                                               CancellationToken token = default,
                                               int channelWidth = 4096)

        {
            List<byte[]> data = new();
            byte[] buffer = new byte[channelWidth];
            int bytesRead = 0;
            bytesRead += await handler.ReceiveAsync(buffer, SocketFlags.None, token);
            data.Add(buffer);

            while (handler.Available > 0 && !token.IsCancellationRequested)
            {
                buffer = new byte[channelWidth];
                bytesRead += await handler.ReceiveAsync(buffer, SocketFlags.None, token);
                data.Add(buffer);
            }

            if (token.IsCancellationRequested) return default;

            var joinedData = data.SelectMany(i => i).ToArray();

            if (joinedData.Length == 0) throw new Exception("Null socket data");

            return new ArraySegment<byte>(joinedData, 0, bytesRead);
        }

        public static async Task<int> Send(Socket handler, object Object)
        {
            var encoded = Encode(Object);
            return await handler.SendAsync(encoded);
        }

        internal static async Task SendContiniousData(Socket handler, Stream dataStream, int channelWidth = 4096)
        {
            byte[] buffer = new byte[channelWidth];
            int bytesRead = dataStream.Read(buffer, 0, channelWidth);
            while (bytesRead > 0)
            {
                await handler.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), SocketFlags.None);
                bytesRead = dataStream.Read(buffer, 0, channelWidth);
            }
            dataStream.Close();
        }
    }
}