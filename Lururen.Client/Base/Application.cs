using Lururen.Client.Audio.Generic;
using Lururen.Client.EntityComponentSystem;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Components;
using Lururen.Client.EntityComponentSystem.Planar.Systems;
using Lururen.Client.Graphics;
using Lururen.Client.Input;
using Lururen.Common.Extensions;
using OpenTK.Windowing.Desktop;

namespace Lururen.Client.Window
{
    using InitEvent = Action;

    public delegate void UpdateEvent(double deltaTime);

    public delegate void ResizeEvent(int width, int height);

    public class Application
    {
        public GLWindow? Window = null;
        public InputManager InputManager { get; private set; }
        public EntityComponentManager EntityManager { get; private set; }
        public WindowSettings Settings { get; private set; } = default;
        public Dictionary<Type, List<ISystem>> Systems { get; private set; } = new();

        public void RegisterSystem<T>(ISystem<T> system) where T : IComponent
        {
            this.Systems.AddOrCreateList(typeof(T), system);
            system.Init(this);
        }

        public void UnregisterSystem<T>(ISystem<T> system) where T : IComponent
        {
            this.Systems.RemoveFromList(system);
            system.Destroy();
        }

        public void RegisterComponent<T>(T component) where T : IComponent
        {
            Systems[typeof(T)].ForEach(system =>
            {
                var castedSystem = system as ISystem<T>;
                castedSystem!.Register(component);
            });
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

        private void CreateWindow()
        {
            Window = new GLWindow(
                            GenerateGameWindowSettings(),
                            GenerateNativeWindowSettings());

            OnLoad = Window.OnLoadEvent;
            OnLoad = Window.OnLoadEvent;
            OnUpdate = Window.OnUpdate;
            OnRender = Window.OnRender;
            OnResizeWindow = Window.OnResizeWindow;
        }

        public void Start(WindowSettings settings = default)
        {
            Settings = settings;

            CreateWindow();

            Window!.VSync = Settings.vSyncMode ?? Window.VSync;

            InputManager = new InputManager(Window);
            EntityManager = EntityComponentManager.GetInstance();

            Window.Run();
        }

        public void Stop()
        {
            Window!.Close();
        }


        protected virtual void Init()
        {
            OnLoad();
        }

        protected virtual void Update(double deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected virtual void Render(double deltaTime)
        {
            OnRender(deltaTime);
        }

        protected virtual void Resize(int width, int height)
        {
            OnResizeWindow(width, height);
        }

        public InitEvent OnLoad { get; set; }
        public UpdateEvent OnUpdate { get; set; }
        public UpdateEvent OnRender { get; set; }
        public ResizeEvent OnResizeWindow { get; set; }
    }

    public class Application2D : Application
    {
        protected override void Init()
        {
            base.Init();
            RegisterSystem(new Renderer2D());
            RegisterSystem(new Camera2DSystem());

            var soundSystem = new SoundSystem();
            RegisterSystem<SoundSource>(soundSystem);
            RegisterSystem<SoundListener>(soundSystem);
        }
    }
}