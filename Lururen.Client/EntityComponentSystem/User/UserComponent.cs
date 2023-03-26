using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.Input;

namespace Lururen.Client.EntityComponentSystem.Components
{
    /// <summary>
    /// Generic user component. Contains everything for interaction with engine.
    /// </summary>
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

        public Entity Instantiate(Prefab prefab, float x = 0, float y = 0, float layer = 0)
        {
            return this.Entity!.World.CreateEntity(prefab);
        }
    }
}