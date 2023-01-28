using Lururen.Client.Graphics.OpenGL.Drawables;

namespace Lururen.Client.Graphics
{
    public interface IContext
    {
        public void DrawElements(double deltaTime);
        public void AddElement(IDrawable drawable);
    }
}