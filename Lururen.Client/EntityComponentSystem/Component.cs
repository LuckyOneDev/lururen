namespace Lururen.Client.ECS
{
    public class Component
    {
        public Entity Entity { get; set; }
        public virtual void Init(Entity entity) 
        { 
            Entity = entity;
        }
        public virtual void Update(double deltaTime) { }
    }
}