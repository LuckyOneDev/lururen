using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.Input;

namespace Lururen.Client.EntityComponentSystem.Components
{
    public abstract class UserComponent : Component
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

        public abstract override void Init();

        public abstract override void Update(double deltaTime);
    }
}