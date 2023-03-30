using Lururen.Client.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lururen.Client.Input
{
    // For later use if we need keypress event
    public delegate void KeyPressEvent(Keys key);

    /// <summary>
    /// Provides access to input devices: mouse, keyboard, controller etc.
    /// </summary>
    public class InputManager
    {
        GLWindow Window { get; }
        public InputManager(GLWindow Window)
        {
            this.Window = Window;
        }

        /// <summary>
        /// Checks if key is down.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if key is down.</returns>
        public bool IsKeyDown(Keys key) => Window.KeyboardState.IsKeyDown(key);

        /// <summary>
        /// Checks if key was released.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if key was released.</returns>
        public bool IsKeyReleased(Keys key) => Window.KeyboardState.IsKeyReleased(key);

        /// <summary>
        /// Checks if key was pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if key was pressed.</returns>
        public bool IsKeyPressed(Keys key) => Window.KeyboardState.IsKeyPressed(key);

        /// <summary>
        /// Gets mouse position on window. Mouse is relative to upper-left corner.
        /// </summary>
        /// <returns>Mouse position.</returns>
        public Vector2 GetMousePos() => Window.MousePosition;
    }
}
