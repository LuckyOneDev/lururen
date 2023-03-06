using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Components;
using Lururen.Client.EntityComponentSystem.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.EntityComponentSystem.Systems
{
    internal class UserComponentSystem : ISystem<UserComponent>
    {
        List<UserComponent> Components { get; set; } = new();
        public Application Application { get; set; }

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
    }
}
