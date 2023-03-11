using Lururen.Client.EntityComponentSystem.Generic;


namespace Lururen.Client.EntityComponentSystem.Base
{
    /// <summary>
    /// Default implementation of IComponent interface.
    /// </summary>
    public abstract class Component : IComponent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Entity? Entity { get; set; }

        public Transform Transform { get; set; }

        public Component(Entity entity)
        {
            Entity = entity;
            Transform = entity.Transform;
        }

        protected void Register<T>(T comp) where T : IComponent
        {
            Entity.World.Application.SystemManager.RegisterComponent(comp); // Wtf this lift
        }

        protected void Unregister<T>(T comp) where T : IComponent
        {
            Entity.World.Application.SystemManager.UnregisterComponent(comp); // Wtf this lift
        }

        public virtual void Dispose()
        {
            Entity = null;
        }

        public abstract void Init();
        public abstract void Update(double deltaTime);

        private bool Active { get; set; } = true;
        Entity? IComponent.Entity { get; set; }

        public void SetActive(bool state)
        {
            Active = state;
        }

        public bool IsActive()
        {
            return Active;
        }
    }
}