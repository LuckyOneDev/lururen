namespace Lururen.Client.EntityComponentSystem.Generic
{
    public interface IComponent : IDisposable
    {
        public Entity? Entity { get; set; }
        public void Init(Entity entity);
        public void Update(double deltaTime);
        public bool Active { get; set; }
    }
}