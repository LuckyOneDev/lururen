using System.Text;

namespace Lururen.Net.Common.Packets.PacketData
{
    public record struct HandshakePacketData(ushort ProtocolVersion, byte HandShakeType) : IPacketData
    {
        public HandshakePacketData(Span<byte> bytes) : this(
                ProtocolVersion: BitConverter.ToUInt16(bytes[..sizeof(ushort)]),
                HandShakeType: bytes[sizeof(ushort)])
        {

        }
        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(ProtocolVersion)
                    .Append(HandShakeType)
                    .ToArray();
        }
    }

    public record struct ServerInfoPacketData(ushort ProtocolVersion, string Info) : IPacketData
    {
        public ServerInfoPacketData(Span<byte> bytes) : this(
                ProtocolVersion: BitConverter.ToUInt16(bytes[..sizeof(ushort)]),
                Info: Encoding.UTF8.GetString(bytes[sizeof(ushort)..]))
        {
        }

        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(ProtocolVersion)
                    .Concat(Encoding.UTF8.GetBytes(Info))
                    .ToArray();
        }
    }
}