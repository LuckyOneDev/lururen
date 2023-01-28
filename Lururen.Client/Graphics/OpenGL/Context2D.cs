using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lururen.Client.Graphics.OpenGL.Drawables;

namespace Lururen.Client.Graphics.OpenGL
{
    public class Context2D : IContext
    {
        public List<IDrawable> Drawables { get; set; } = new();

        public void AddElement(IDrawable drawable)
        {
            drawable.Init();
            Drawables.Add(drawable);
        }

        public void DrawElements(double deltaTime)
        {
            Drawables.ForEach(drawable => drawable.Draw());
        }
    }
}
