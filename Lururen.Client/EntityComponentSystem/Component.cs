namespace Lururen.Client.ECS
{
    public interface IComponent : IDisposable
    {
        public Entity? Entity { get; set; }
        public void Init(Entity entity);
        public void Update(double deltaTime);
    }

    public abstract class Component : IComponent
    {
        public Entity? Entity { get; set; }

        public virtual void Init(Entity entity) 
        { 
            Entity = entity;
        }

        public virtual void Update(double deltaTime) { }

        public virtual void Dispose()
        {
            Entity = null;
        }
    }
}