using Lururen.Client.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lururen.Client.Input
{
    /// <summary>
    /// Provides access to input devices: mouse, keyboard, controller etc.
    /// </summary>
    public class InputManager
    {
        GLWindow Window { get; }
        public InputManager(GLWindow window)
        {
            this.Window = window;
            this.Keyboard = new Keyboard(window);
            this.Mouse = new Mouse(window);
        }

        public Keyboard Keyboard { get; }
        public Mouse Mouse { get; }
    }
}
