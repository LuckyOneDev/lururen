using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Generic;

namespace Lururen.Client.EntityComponentSystem.User
{
    internal class UserComponentSystem : ISystem<UserComponent>
    {
        List<UserComponent> Components { get; set; } = new();
        public Application Application { get; set; }

        public void Init(Application application)
        {
            Application = application;
            Application.Window!.OnUpdate += Update;
        }
        public void Register(UserComponent component)
        {
            Components.Add(component);
        }

        public void Unregister(UserComponent component)
        {
            Components.Remove(component);
        }

        public void Update(double deltaTime)
        {
            foreach (var item in Components)
            {
                item.Update(deltaTime);
            }
        }

        public void Destroy()
        {
        }
    }
}
