using Lururen.Client.Graphics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Planar.Components
{
    public class DrawableComponent : Component, Drawable
    {
        string ShaderPath { get; set; }
        string TexturePath { get; set; }

        public string GetShaderPath()
        {
            return ShaderPath;
        }

        public string GetTexturePath()
        {
            return TexturePath;
        }
    }

    public interface Drawable
    {
        public string GetShaderPath();
        public string GetTexturePath();
    }
}
