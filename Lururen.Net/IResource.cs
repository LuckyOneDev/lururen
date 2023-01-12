using System.Security.AccessControl;

namespace Lururen.Net
{
    public enum ResourceType
    {
        CScript,
        PNG,
        OGG
    }

    public interface IResource
    {
        public Uri Uri { get; }
        public byte[] Data { get; }
        public ResourceType Type { get; }
    }
}