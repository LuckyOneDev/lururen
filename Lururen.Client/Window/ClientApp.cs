using Lururen.Client.EntityComponentSystem;
using Lururen.Client.EntityComponentSystem.Generic;
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
        public ISystem<Camera2D> CameraManager { get; private set; }
        public WindowSettings Settings { get; private set; }

        private GameWindowSettings GenerateGameWindowSettings()
        {
            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.RenderFrequency = Settings.UpdateFrequency ?? gameWindowSettings.RenderFrequency;
            gameWindowSettings.UpdateFrequency = Settings.UpdateFrequency ?? gameWindowSettings.UpdateFrequency;
            return gameWindowSettings;
        }

        private NativeWindowSettings GenerateNativeWindowSettings()
        {
            NativeWindowSettings nativeWindowSettings = NativeWindowSettings.Default;
            nativeWindowSettings.Title = Settings.Title ?? nativeWindowSettings.Title;
            nativeWindowSettings.WindowState = Settings.WindowState ?? nativeWindowSettings.WindowState;
            nativeWindowSettings.Icon = Settings.Icon ?? nativeWindowSettings.Icon;
            nativeWindowSettings.Size = Settings.Size ?? nativeWindowSettings.Size;

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

            Window.VSync = Settings.vSyncMode ?? Window.VSync;

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