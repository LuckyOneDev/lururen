using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.EntityComponentSystem.Components
{
    public class UserComponent : Component
    {
        protected InputManager Input;

        public UserComponent(Entity entity) : base(entity)
        {
            Register(this);
            Input = entity.World.Application.InputManager;
        }

        public override void Dispose()
        {
            Unregister(this);
            base.Dispose();
        }
    }
}
