namespace Lururen.Client.ECS
{
    public interface IComponent
    {
        public Entity Entity { get; set; }
        public void Init();
        public void Update(double deltaTime);
    }
}