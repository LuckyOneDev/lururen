namespace Lururen.Net.Common.Packets.PacketData
{

    public record LoginStartPacketData(Guid PlayerId) : IPacketData
    {
        public LoginStartPacketData(Span<byte> bytes) : this(
                PlayerId: new Guid(bytes[..16]))
        {
        }

        public byte[] ToBytes()
        {
            return PlayerId.ToByteArray();
        }
    }

    public record LoginSuccessPacketData(int ErrCode) : IPacketData
    {
        public LoginSuccessPacketData(Span<byte> bytes) : this(
                ErrCode: BitConverter.ToInt32(bytes[..4]))
        {
        }

        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(ErrCode);
        }
    }

    public record LoginRejectPacketData(int ErrCode) : IPacketData
    {
        public LoginRejectPacketData(Span<byte> bytes) : this(
                ErrCode: BitConverter.ToInt32(bytes[..4]))
        {
        }

        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(ErrCode);
        }
    }

    public record ConfigureChannelPacketData(bool UseCompression, bool UseEncryption, long EncryptionPublicKey) : IPacketData
    {
        public ConfigureChannelPacketData(Span<byte> bytes) : this(
                UseCompression: BitConverter.ToBoolean(bytes[..1]),
                UseEncryption: BitConverter.ToBoolean(bytes[1..2]),
                EncryptionPublicKey: 0)
        {
            EncryptionPublicKey = UseEncryption ? BitConverter.ToInt64(bytes[2..10]) : 0;
        }

        public byte[] ToBytes()
        {
            IEnumerable<byte> result = BitConverter.GetBytes(UseCompression)
                    .Concat(BitConverter.GetBytes(UseEncryption));
            if (UseEncryption)
            {
                result = result.Concat(BitConverter.GetBytes(EncryptionPublicKey));
            }
            return result.ToArray();
        }
    }

    public record ConfigureChannelResponsePacketData(int ErrCode) : IPacketData
    {
        public ConfigureChannelResponsePacketData(Span<byte> bytes) : this(
                ErrCode: BitConverter.ToInt32(bytes[..4]))
        {
        }

        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(ErrCode);
        }
    }
}