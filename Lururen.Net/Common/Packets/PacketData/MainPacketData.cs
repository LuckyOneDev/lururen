namespace Lururen.Net.Common.Packets.PacketData
{
    public record ReadyResponsePacketData(byte[] Data) : IPacketData
    {
        public ReadyResponsePacketData(Span<byte> bytes) : this(
                Data: bytes.ToArray())
        {
        }
        public byte[] ToBytes()
        {
            return Data;
        }
    }
}