using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace Lururen.Common.Networking.SocketNetworking
{
    public static class SocketHelper
    {
        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private static readonly Encoding Encoding = Encoding.Unicode;

        private static byte[] Encode(object Object)
        {
            string jsonResponse = JsonConvert.SerializeObject(Object, JsonSettings);
            return Encoding.GetBytes(jsonResponse);
        }

        private static T Decode<T>(ArraySegment<byte> binaryObject) where T : class
        {
            string stringObject = Encoding.GetString(binaryObject);
            return JsonConvert.DeserializeObject<T>(stringObject, JsonSettings);
        }

        public static async Task<T> Recieve<T>(this Socket handler,
                                               CancellationToken token = default,
                                               int channelWidth = 4096) where T : class

        {
            ArraySegment<byte> bytes = await handler.RecieveBytes(token, channelWidth);
            return Decode<T>(bytes);
        }

        public static async Task<ArraySegment<byte>> RecieveBytes(this Socket handler,
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

            if (token.IsCancellationRequested)
            {
                return default;
            }

            byte[] joinedData = data.SelectMany(i => i).ToArray();

            if (!joinedData.Any())
            {
                throw new InvalidDataException("Null socket data");
            }

            return new ArraySegment<byte>(joinedData, 0, bytesRead);
        }

        public static async Task<int> Send(this Socket handler, object Object)
        {
            byte[] encoded = Encode(Object);
            return await handler.SendAsync(encoded);
        }

        public static async Task SendContiniousData(this Socket handler, Stream dataStream, int channelWidth = 4096)
        {
            byte[] buffer = new byte[channelWidth];
            int bytesRead = dataStream.Read(buffer, 0, channelWidth);
            while (bytesRead > 0)
            {
                _ = await handler.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), SocketFlags.None);
                bytesRead = dataStream.Read(buffer, 0, channelWidth);
            }
            dataStream.Close();
        }
    }
}