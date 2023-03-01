using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;

namespace Lururen.Client.Base
{
    public record WindowSettings
    {
        public VSyncMode? vSyncMode { get; set; }
        public string? Title { get; set; }
        public double? UpdateFrequency { get; set; }
        public WindowState? WindowState { get; set; }
        public WindowIcon? Icon { get; set; }
        public Vector2i? Size { get; set; }

        internal GameWindowSettings GameWindowSettings()
        {
            GameWindowSettings gameWindowSettings = OpenTK.Windowing.Desktop.GameWindowSettings.Default;
            gameWindowSettings.RenderFrequency = UpdateFrequency ?? gameWindowSettings.RenderFrequency;
            gameWindowSettings.UpdateFrequency = UpdateFrequency ?? gameWindowSettings.UpdateFrequency;

            return gameWindowSettings;
        }

        internal NativeWindowSettings NativeWindowSettings()
        {
            NativeWindowSettings nativeWindowSettings = OpenTK.Windowing.Desktop.NativeWindowSettings.Default;
            nativeWindowSettings.Title = Title ?? nativeWindowSettings.Title;
            nativeWindowSettings.WindowState = WindowState ?? nativeWindowSettings.WindowState;
            nativeWindowSettings.Icon = Icon ?? nativeWindowSettings.Icon;
            nativeWindowSettings.Size = Size ?? nativeWindowSettings.Size;

            return nativeWindowSettings;
        }
    }
}