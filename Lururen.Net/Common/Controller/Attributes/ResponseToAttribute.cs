using Lururen.Net.Common.Packets;

namespace Lururen.Net.Common.Controller.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ResponseToAttribute : Attribute
    {
        public PacketType PacketType { get; }

        public ResponseToAttribute(PacketType packetType)
        {
            PacketType = packetType;
        }
    }
}