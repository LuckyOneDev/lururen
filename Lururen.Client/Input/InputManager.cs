﻿using Lururen.Client.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lururen.Client.Input
{
    // For later use if we need keypress event
    public delegate void KeyPressEvent(Keys key);
    public class InputManager
    {
        OpenGLWindow Window { get; set; }
        public InputManager(OpenGLWindow Window) 
        { 
            this.Window = Window;
        }

        public bool IsKeyDown(Keys key) => Window.KeyboardState.IsKeyDown(key);
        public bool IsKeyReleased(Keys key) => Window.KeyboardState.IsKeyReleased(key);
        public bool IsKeyPressed(Keys key) => Window.KeyboardState.IsKeyPressed(key);
        public Vector2 GetMousePos() => Window.MousePosition;
        public void Update()
        {

        }
    }
}