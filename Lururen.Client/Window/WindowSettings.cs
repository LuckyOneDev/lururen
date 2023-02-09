using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;

namespace Lururen.Client.Window
{
    public struct WindowSettings
    {
        public VSyncMode vSyncMode { get; set; }
        public string Title { get; set; }
        public double UpdateFrequency { get; set; }
        public WindowState WindowState { get; set; }
        public WindowIcon Icon { get; set; }
        public Vector2i Size { get; set; }
    }
}