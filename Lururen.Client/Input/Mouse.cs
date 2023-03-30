using Lururen.Client.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lururen.Client.Input
{
    public class Mouse
    {
        GLWindow Window { get; }
        internal Mouse(GLWindow Window)
        {
            this.Window = Window;
        }

        /// <summary>
        /// Gets mouse position on window. Mouse is relative to upper-left corner.
        /// </summary>
        /// <returns>Mouse position.</returns>
        public Vector2 Position => Window.MousePosition;

        public void SetMouseCursorMode(CursorModeValue mode)
        {
            unsafe
            {
                GLFW.SetInputMode(Window.WindowPtr, CursorStateAttribute.Cursor, mode);
            }
        }
    }
}
