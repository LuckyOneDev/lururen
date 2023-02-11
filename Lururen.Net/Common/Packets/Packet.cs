using System;
using Lururen.Net.Common.Packets.PacketData;

namespace Lururen.Net.Common.Packets
{
    /// <summary>
    /// A packet type. This is used to define data contract of sending data.
    /// Actual data is stored in <see cref="IPacketData"/>.
    /// </summary>
    public record struct Packet(PacketType PacketType, IPacketData? Data)
    {
        public uint PacketSize => (uint)(Data is null ? 0 : Data.ToBytes().Length);

        public byte[] ToBytes()
        {
            IEnumerable<byte> result = BitConverter.GetBytes(PacketSize)
                    .Append((byte)PacketType);

            if (Data is not null)
            {
                result = result.Concat(Data.ToBytes());
            }
            return result.ToArray();
        }

        public static async Task<Packet?> FromStream(Stream stream)
        {
            byte[] size = new byte[4];
            byte[] type = new byte[1];

            int sizseRead = await stream.ReadAsync(size.AsMemory(0, 4));
            int typeRead = await stream.ReadAsync(type.AsMemory(0, 1));

            if (sizseRead != 4 || typeRead != 1)
            {
                return null;
            }

            uint packetSize = BitConverter.ToUInt32(size);
            PacketType packetType = (PacketType)type[0];

            byte[] data = new byte[packetSize];
            int dataRead = await stream.ReadAsync(data.AsMemory(0, (int)packetSize));

            if (dataRead != packetSize)
            {
                return new Packet(packetType, null);
            }

            IPacketData? packetData = packetType switch
            {
                PacketType.Handshake => new HandshakePacketData(data),
                PacketType.ServerInfo => new ServerInfoPacketData(data),
                PacketType.LoginStart => new LoginStartPacketData(data),
                PacketType.LoginSuccess => new LoginSuccessPacketData(data),
                PacketType.LoginReject => new LoginRejectPacketData(data),
                PacketType.ConfigureChannel => new ConfigureChannelPacketData(data),
                PacketType.ConfigureChannelResponse => new ConfigureChannelResponsePacketData(data),
                PacketType.RequestAvailableResources => null,
                PacketType.ResponseAvailableResources => new ResponseAvailableResourcesPacketData(data),
                PacketType.RequestResource => new RequestResourcePacketData(data),
                PacketType.Resource => new Resource(data),
                PacketType.ResourceExchangeEnd => null,
                PacketType.Ready => null,
                PacketType.ReadyResponse => new ReadyResponsePacketData(data),
                PacketType.Disconnect => null,
                _ => null
            };

            return new Packet(packetType, packetData);

        }
    }



}