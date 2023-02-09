using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
