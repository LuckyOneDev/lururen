using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Graphics.OpenGL.Drawables
{
    public interface IDrawable
    {
        public void Draw();
        public void Init();
    }
}
