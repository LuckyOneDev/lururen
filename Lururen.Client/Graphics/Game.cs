using Lururen.Client.Input;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Graphics
{
    public delegate void FrameUpdateEvent(double deltaTime);
    public delegate void ResizeEvent(int width, int height);
    public class Game : GameWindow
    {
        private FrameUpdateEvent OnUpdate { get; set; }
        private FrameUpdateEvent OnRender { get; set; }
        private ResizeEvent OnResizeWindow { get; set; }
        private Action OnLoadEvent { get; set; }

        public Game(FrameUpdateEvent onUpdateFrame, FrameUpdateEvent onRender, ResizeEvent onResize, Action onLoad) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            OnUpdate = onUpdateFrame;
            OnRender = onRender;
            OnResizeWindow = onResize;
            OnLoadEvent = onLoad;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            OnUpdate.Invoke(args.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            OnRender.Invoke(e.Time);
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            OnResizeWindow.Invoke(e.Width, e.Height);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            OnLoadEvent.Invoke();
        }
    }
}
