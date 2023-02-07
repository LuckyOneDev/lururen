using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Systems;
using Lururen.Client.Graphics;
using Lururen.Client.Input;
using System.Diagnostics;

namespace Lururen.Client
{
    public class ClientApp
    {
        public Game? Window = null;
        public InputManager Keyboard { get; private set; }
        public EntityManager EntityManager { get; private set; }
        public Renderer2D SpriteRenderer { get; private set; }
        public CameraSystem CameraSystem { get; private set; }
        public void Start()
        {
            Window = new Game(Update, Render, Resize, Init);
            this.SpriteRenderer = Renderer2D.GetInstance();
            this.CameraSystem = CameraSystem.GetInstance();
            this.Keyboard = new InputManager(Window);
            this.EntityManager = new EntityManager();
            Window.Run();
        }

        public void Stop()
        {
            Window.Close();
        }

        public virtual void Init()
        {
            SpriteRenderer.Init(Window);
        }

        public virtual void Update(double deltaTime)
        {
        }

        public virtual void Render(double deltaTime)
        {
            Debug.WriteLine(1 / deltaTime); // fps
            this.CameraSystem.Update(deltaTime);
            this.SpriteRenderer.Update(deltaTime);
        }

        public virtual void Resize(int width, int height)
        {
        }
    }
}