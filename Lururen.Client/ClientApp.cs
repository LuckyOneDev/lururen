using Lururen.Client.Graphics;
using Lururen.Client.Graphics.OpenGL;
using Lururen.Client.Input;

namespace Lururen.Client
{
    public class ClientApp
    {
        public OpenGLWindow? Window = null;

        public InputManager KeyboardInputManager { get; private set; }
        public EntityManager EntityManager { get; private set; }
        public IContext RenderingContext { get; private set; }

        public void Start(IContext RenderingContext)
        {
            Window = new OpenGLWindow(Update, Render, Resize, Init);
            this.RenderingContext = RenderingContext;
            this.KeyboardInputManager = new InputManager(Window);
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
            this.EntityManager.Update(deltaTime);
        }

        public virtual void Render(double deltaTime)
        {
            this.RenderingContext.DrawElements(deltaTime);
        }

        public virtual void Resize(int width, int height)
        {
        }
    }
}