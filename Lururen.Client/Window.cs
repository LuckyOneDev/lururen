using Lururen.Client.Input;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client
{
    public delegate void OnUpdateFrame(double deltaTime);
    public class Window : GameWindow
    {
        private OnUpdateFrame OnUpdate { get; set; }
        public Window(OnUpdateFrame onUpdateFrame) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.OnUpdate = onUpdateFrame;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            OnUpdate.Invoke(args.Time);
        }
    }
}
