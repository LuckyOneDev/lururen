using Lururen.Client.Graphics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Input
{
    public class Keyboard
    {
        GLWindow Window { get; }
        internal Keyboard(GLWindow Window)
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
    }
}
