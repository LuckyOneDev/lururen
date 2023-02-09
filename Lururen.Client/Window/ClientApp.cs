using Lururen.Client.EntityComponentSystem;
using Lururen.Client.EntityComponentSystem.Planar.Components;
using Lururen.Client.EntityComponentSystem.Planar.Systems;
using Lururen.Client.Graphics;
using Lururen.Client.Input;
using OpenTK.Windowing.Desktop;

namespace Lururen.Client.Window
{
    public class ClientApp
    {
        public Game? Window = null;
        public InputManager InputManager { get; private set; }
        public EntityManager EntityManager { get; private set; }
        public IRenderSystem<SpriteRenderer> RenderSystem { get; private set; }
        public ISystem<Camera> CameraManager { get; private set; }
        public WindowSettings Settings { get; private set; }

        private GameWindowSettings GenerateGameWindowSettings()
        {
            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.RenderFrequency = Settings.UpdateFrequency == default ? gameWindowSettings.RenderFrequency : Settings.UpdateFrequency;
            gameWindowSettings.UpdateFrequency = Settings.UpdateFrequency == default ? gameWindowSettings.UpdateFrequency : Settings.UpdateFrequency;
            return gameWindowSettings;
        }

        private NativeWindowSettings GenerateNativeWindowSettings()
        {
            NativeWindowSettings nativeWindowSettings = NativeWindowSettings.Default;
            nativeWindowSettings.Title = Settings.Title == default ? nativeWindowSettings.Title : Settings.Title;
            nativeWindowSettings.WindowState = Settings.WindowState == default ? nativeWindowSettings.WindowState : Settings.WindowState;
            nativeWindowSettings.Icon = Settings.Icon == default ? nativeWindowSettings.Icon : Settings.Icon;
            nativeWindowSettings.Size = Settings.Size == default ? nativeWindowSettings.Size : Settings.Size;

            return nativeWindowSettings;
        }

        public void Start(WindowSettings settings = default)
        {
            Settings = settings;

            Window = new Game(
                Update,
                Render,
                Resize,
                Init,
                GenerateGameWindowSettings(),
                GenerateNativeWindowSettings());

            Window.VSync = Settings.vSyncMode;

            RenderSystem = Renderer2D.GetInstance();
            CameraManager = Camera2DSystem.GetInstance();
            InputManager = new InputManager(Window);
            EntityManager = EntityManager.GetInstance();

            Window.Run();
        }

        public void Stop()
        {
            Window.Close();
        }

        public virtual void Init()
        {
            RenderSystem.Init(Window);
        }

        public virtual void Update(double deltaTime)
        {
        }

        public virtual void Render(double deltaTime)
        {
            CameraManager.Update(deltaTime);
            RenderSystem.Update(deltaTime);
        }

        public virtual void Resize(int width, int height)
        {
        }
    }
}