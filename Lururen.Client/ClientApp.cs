using Lururen.Client.Graphics;
using Lururen.Client.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client
{
    public class ClientApp
    {
        public Window? Window = null;

        public InputManager KeyboardInputManager { get; private set; }
        public EntityManager EntityManager { get; private set; }
        public void Start()
        {
            Window = new Window(Update);
            this.KeyboardInputManager = new InputManager(Window);
            this.EntityManager = new EntityManager();
            Init();
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
            this.EntityManager.Update();
        }
    }
}
