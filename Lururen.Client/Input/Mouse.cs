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

        public bool IsButtonDown(MouseButton button) => Window.IsMouseButtonDown(button);
        public bool IsButtonPressed(MouseButton button) => Window.IsMouseButtonPressed(button);
        public bool IsButtonReleased(MouseButton button) => Window.IsMouseButtonReleased(button);
        public bool IsAnyButtonDown() => Window.IsAnyMouseButtonDown;
        public Vector2 Delta => Window.MouseState.Delta;
        public Vector2 ScrollDelta => Window.MouseState.ScrollDelta;

        public void SetMouseCursorMode(CursorModeValue mode)
        {
            unsafe
            {
                GLFW.SetInputMode(Window.WindowPtr, CursorStateAttribute.Cursor, mode);
            }
        }
    }
}
