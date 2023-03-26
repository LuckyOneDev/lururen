using Lururen.Client.ResourceManagement;

namespace Lururen.Client.Audio
{
    public class Sound
    {
        internal FileAccessor Accessor;
        public Sound(string path, ResourceLocation location = ResourceLocation.FileSystem)
        {
            Accessor = new FileAccessor(
                Path: path,
                ResourceLocation: location
            );
        }
    }
}
