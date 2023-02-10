using Lururen.Client.EntityComponentSystem.Generic;

namespace Lururen.Client.EntityComponentSystem
{
    /// <summary>
    /// Default implementation of IComponent interface.
    /// </summary>
    public abstract class Component : IComponent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Entity? Entity { get; set; }
        public bool Active { get; set; } = true;

        public Component(Entity entity)
        {
            Entity = entity;
            EntityComponentManager.GetInstance().AddComponent(this);
        }

        public virtual void Init() { }

        public virtual void Update(double deltaTime) { }

        public virtual void Dispose()
        {
            Entity = null;
            EntityComponentManager.GetInstance().RemoveComponent(this);
        }
    }
}