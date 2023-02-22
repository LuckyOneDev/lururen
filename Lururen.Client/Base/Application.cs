using Lururen.Client.EntityComponentSystem;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Components;
using Lururen.Client.EntityComponentSystem.Planar.Systems;
using Lururen.Client.Graphics;
using Lururen.Client.Input;
using OpenTK.Windowing.Desktop;

namespace Lururen.Client.Window
{
    public class Application
    {
        public GLWindow? Window = null;
        public InputManager InputManager { get; private set; }
        public EntityComponentManager EntityManager { get; private set; }
        public WindowSettings Settings { get; private set; }
        public List<ISystem<IComponent>> Systems { get; private set; } = new();

        public void Register(ISystem<IComponent> system)
        {
            this.Systems.Add(system);
            system.Init(this);
        }

        public void Unregister(ISystem<IComponent> system)
        {
            this.Systems.Remove(system);
            system.Destroy();
        }

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

            Window = new GLWindow(
                Update,
                Render,
                Resize,
                Init,
                GenerateGameWindowSettings(),
                GenerateNativeWindowSettings());

            Window.VSync = Settings.vSyncMode ?? Window.VSync;

            // RenderSystem = (IRenderSystem)Renderer2D.GetInstance();
            // CameraManager = Camera2DSystem.GetInstance();
            // SoundSystem = EntityComponentSystem.Planar.Systems.SoundSystem.GetInstance();

            InputManager = new InputManager(Window);
            EntityManager = EntityComponentManager.GetInstance();
            Window.Run();
        }

        public void Stop()
        {
            Window.Close();
        }

        public virtual void Init()
        {
            //RenderSystem.Init(Window);
            //SoundSystem.Init();
        }

        public virtual void Update(double deltaTime)
        {
            //SoundSystem.Update(deltaTime);
        }

        public virtual void Render(double deltaTime)
        {
            //CameraManager.Update(deltaTime);
            //RenderSystem.Update(deltaTime);
        }

        public virtual void Resize(int width, int height)
        {
        }
    }
}