using System.Text;

namespace Lururen.Net.Common.Packets.PacketData
{
    public record ResponseAvailableResourcesPacketData(ResourceInfo[] AvailableResources) : IPacketData
    {
        public ResponseAvailableResourcesPacketData(Span<byte> bytes) : this(
                AvailableResources: Array.Empty<ResourceInfo>())
        {
            List<ResourceInfo> resources = new();
            int offset = 0;
            while (offset < bytes.Length)
            {
                ResourceInfo resource = new(bytes[offset..]);
                resources.Add(resource);
                offset += resource.ToBytes().Length;
            }
            AvailableResources = resources.ToArray();
        }

        public byte[] ToBytes()
        {
            return AvailableResources.SelectMany(x => x.ToBytes()).ToArray();
        }
    }


    public record struct ResourceInfo(int Type, long Hash, string Name) : IPacketData
    {
        public ResourceInfo(Span<byte> bytes) : this(
                Type: BitConverter.ToInt32(bytes[..4]),
                Hash: BitConverter.ToInt64(bytes[4..12]),
                Name: "")
        {
            int nameLength = BitConverter.ToInt32(bytes[12..16]);
            Name = Encoding.UTF8.GetString(bytes[16..(16 + nameLength)]);
        }
        public byte[] ToBytes()
        {
            IEnumerable<byte> result = BitConverter.GetBytes(Type)
                    .Concat(BitConverter.GetBytes(Hash))
                    .Concat(BitConverter.GetBytes(Name.Length));
            return result.Concat(Encoding.UTF8.GetBytes(Name)).ToArray();
        }
    }

    public record RequestResourcePacketData(string Name) : IPacketData
    {
        public RequestResourcePacketData(Span<byte> bytes) : this(
                Name: Encoding.UTF8.GetString(bytes))
        {
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(Name);
        }
    }

    public record Resource(byte[] Data) : IPacketData
    {
        public Resource(Span<byte> bytes) : this(
                Data: bytes.ToArray())
        {
        }

        public byte[] ToBytes()
        {
            return Data;
        }
    }
}


