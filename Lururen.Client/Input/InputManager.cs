using Lururen.Client.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lururen.Client.Input
{
    // For later use if we need keypress event
    public delegate void KeyPressEvent(Keys key);
    public class InputManager
    {
        Game Window { get; set; }
        public InputManager(Game Window) 
        { 
            this.Window = Window;
        }

        public bool IsKeyDown(Keys key) => Window.KeyboardState.IsKeyDown(key);
        public bool IsKeyReleased(Keys key) => Window.KeyboardState.IsKeyReleased(key);
        public bool IsKeyPressed(Keys key) => Window.KeyboardState.IsKeyPressed(key);
        public Vector2 GetMousePos() => Window.MousePosition;
        //public Action<KeyboardKeyEventArgs> KeyDown => Window.KeyDown;
        public void Update()
        {

        }
    }
}
