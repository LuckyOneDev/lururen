using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Systems;
using Lururen.Client.Graphics;
using Lururen.Client.Input;

namespace Lururen.Client
{
    public class ClientApp
    {
        public Game? Window = null;
        public InputManager Keyboard { get; private set; }
        public EntityManager EntityManager { get; private set; }
        public Context2D RenderingContext { get; private set; }

        public void Start(Context2D RenderingContext)
        {
            Window = new Game(Update, Render, Resize, Init);
            this.RenderingContext = RenderingContext;
            RenderingContext.Init(Window);
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
        }

        public virtual void Update(double deltaTime)
        {
        }

        public virtual void Render(double deltaTime)
        {
            this.RenderingContext.Update(deltaTime);
        }

        public virtual void Resize(int width, int height)
        {
        }
    }
}