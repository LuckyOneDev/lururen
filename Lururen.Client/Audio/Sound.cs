using Lururen.Client.Audio.Generic;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.ResourceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
