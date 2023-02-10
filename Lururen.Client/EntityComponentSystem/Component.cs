using Lururen.Client.EntityComponentSystem.Generic;

namespace Lururen.Client.EntityComponentSystem
{

    public abstract class Component : IComponent
    {
        public Entity? Entity { get; set; }
        public bool Active { get; set; }

        public Component(Entity entity)
        {
            Entity = entity;
        }

        public virtual void Init(Entity entity) { }

        public virtual void Update(double deltaTime) { }

        public virtual void Dispose()
        {
            Entity = null;
        }
    }
}