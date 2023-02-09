using OpenTK.Graphics.OpenGL4;

namespace Lururen.Client.Graphics.Texturing
{
    public struct TextureParameters
    {
        public TextureMagFilter TextureMagFilter = TextureMagFilter.Linear;
        public TextureMinFilter TextureMinFilter = TextureMinFilter.Linear;

        public TextureWrapMode TextureWrapS = TextureWrapMode.Repeat;
        public TextureWrapMode TextureWrapT = TextureWrapMode.Repeat;

        public bool GenerateMipMap = true;
        public TextureParameters()
        {
        }
    }
}
