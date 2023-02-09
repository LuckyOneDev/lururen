using Lururen.Client.Graphics.Generic;
using Lururen.Client.ResourceManagement;
using SixLabors.ImageSharp;

namespace Lururen.Client.Graphics.Texturing
{
    public class Texture2D
    {
        public int Width, Height;
        internal FileAccessor Accessor;
        public Texture2D(string path, ResourceLocation location = ResourceLocation.FileSystem)
        {
            Accessor = new FileAccessor()
            {
                Path = path,
                ResourceLocation = location
            };

            var texture = FileHandle<GLTexture>.GetInstance().Get(Accessor);
            Width = texture.Width;
            Height = texture.Height;
        }

        public void SetPixel(int x, int y, Color color)
        {

        }
    }
}