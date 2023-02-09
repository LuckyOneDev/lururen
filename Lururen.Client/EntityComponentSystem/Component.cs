namespace Lururen.Client.EntityComponentSystem
{
    public interface IComponent : IDisposable
    {
        public Entity? Entity { get; set; }
        public void Init(Entity entity);
        public void Update(double deltaTime);
        public bool Active { get; set; }
    }

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