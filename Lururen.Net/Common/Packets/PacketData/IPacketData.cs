namespace Lururen.Net.Common.Packets.PacketData
{
    /// <summary>
    /// Packet data type. General type to describe various packet contents.
    /// </summary>
    public interface IPacketData
    {
        public byte[] ToBytes();
    }
}