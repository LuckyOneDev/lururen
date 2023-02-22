using Lururen.Client.Window;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Lururen.Client.Graphics
{
    public class GLWindow : GameWindow
    {
        public UpdateEvent OnUpdate { get; set; }
        public UpdateEvent OnRender { get; set; }
        public ResizeEvent OnResizeWindow { get; set; }
        public Action OnLoadEvent { get; set; }

        public GLWindow(
                GameWindowSettings gameWindowSettings,
                NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            OnUpdate?.Invoke(args.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            OnRender?.Invoke(e.Time);
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            OnResizeWindow?.Invoke(e.Width, e.Height);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            OnLoadEvent?.Invoke();
        }
    }
}
