using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.EntityComponentSystem.Components;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Systems;
using Lururen.Client.Graphics;
using Lururen.Client.Input;
using Lururen.Common.Extensions;
using OpenTK.Windowing.Desktop;

namespace Lururen.Client.Base
{
    using InitEvent = Action;

    public delegate void UpdateEvent(double deltaTime);

    public delegate void ResizeEvent(int width, int height);

    public class SystemManager
    {
        public SystemManager(Application application)
        {
            Application = application;
        }

        public Application Application { get; }
        public Dictionary<Type, List<ISystem>> Systems { get; private set; } = new();
        public void RegisterSystem<T>(ISystem<T> system) where T : IComponent
        {
            Systems.AddOrCreateList(typeof(T), system);
            system.Init(Application);
        }

        public void UnregisterSystem<T>(ISystem<T> system) where T : IComponent
        {
            Systems.RemoveFromList(system);
            system.Destroy();
        }

        /// <summary>
        /// Reroutes component register calls.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        public void RegisterComponent<T>(T component) where T : IComponent
        {
            Systems[typeof(T)].ForEach(system =>
            {
                var castedSystem = system as ISystem<T>;
                castedSystem!.Register(component);
            });
        }

        /// <summary>
        /// Reroutes component unregister calls.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        public void UnregisterComponent<T>(T component) where T : IComponent
        {
            Systems[typeof(T)].ForEach(system =>
            {
                var castedSystem = system as ISystem<T>;
                castedSystem!.Unregister(component);
            });
        }

        public List<T> GetSystems<T>()
        {
            return Systems[typeof(T)] as List<T>;
        }
    }

    public class WorldManager
    {
        public WorldManager(Application application)
        {
            Application = application;
        }

        public Application Application { get; }

        public World Active { get; private set; }
        List<World> Worlds { get; } = new();

        private void SetActive(World world)
        {
            Active = world;
        }

        public void SetActiveWorld(string worldId)
        {
            var found = Worlds.Find(x => x.Id == worldId);
            if (found is not null)
            {
                SetActive(found);
            }
            else
            {
                throw new Exception();
            }
        }

        public void Create(string worldId)
        {
            Worlds.Add(new World(Application, worldId));
        }
    }
    public class Application
    {
        public Application()
        {
            SystemManager = new(this);
            WorldManager = new(this);
        }

        public WindowSettings Settings { get; private set; } = default;
        internal GLWindow? Window { get; set; } = null;
        public InputManager InputManager { get; private set; }
        public EntityComponentManager EntityManager { get; private set; }
        public SystemManager SystemManager { get; }
        public WorldManager WorldManager { get; }

        private GLWindow? CreateWindow()
        {
            var Window = new GLWindow(
                            Settings.GameWindowSettings(),
                            Settings.NativeWindowSettings());

            Window.OnLoadEvent += Init;
            Window.OnUpdate += Update;
            Window.OnRender += Render;
            Window.OnResizeWindow += Resize;

            return Window;
        }

        public void Start(WindowSettings settings = default)
        {
            Settings = settings;

            Window = CreateWindow();

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
        }

        protected virtual void Update(double deltaTime)
        {
        }

        protected virtual void Render(double deltaTime)
        {
        }

        protected virtual void Resize(int width, int height)
        {
        }

        public Entity Instantiate(Prefab prefab)
        {
            return WorldManager.Active.CreateEntity(prefab);
        }
    }

    public class Application2D : Application
    {
        public Application2D()
        {
            WorldManager.Create("default");
            WorldManager.SetActiveWorld("default");
        }

        protected override void Init()
        {
            base.Init();

            var spriteRender = new SpriteRenderSystem();
            var camSystem = new CameraSystem();
            var soundSystem = new SoundSystem();
            var ucSystem = new UserComponentSystem();

            spriteRender.BindSystems(Window, camSystem);

            SystemManager.RegisterSystem(spriteRender);
            SystemManager.RegisterSystem(camSystem);
            SystemManager.RegisterSystem<SoundSource>(soundSystem);
            SystemManager.RegisterSystem<SoundListener>(soundSystem);
            SystemManager.RegisterSystem(ucSystem);
        }
    }
}