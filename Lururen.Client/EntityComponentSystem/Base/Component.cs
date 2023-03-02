using Lururen.Client.EntityComponentSystem.Generic;


namespace Lururen.Client.EntityComponentSystem.Base
{
    /// <summary>
    /// Default implementation of IComponent interface.
    /// </summary>
    public class Component : IComponent
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

        public virtual void Update(double deltaTime) { }

        public virtual void Dispose()
        {
            Entity = null;
        }

        public virtual void Init<T>(ISystem<T> system) where T : IComponent
        {
            if (this is T thisCasted)
            {
                system.Register(thisCasted);
            }
            else
            {
                throw new ArgumentException("System was not of corresponding type");
            }
        }

        private bool Active { get; set; } = true;
        Entity? IComponent.Entity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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