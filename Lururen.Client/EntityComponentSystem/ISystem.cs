using Lururen.Client.Graphics;

namespace Lururen.Client.ECS
{
    public interface ISystem<T> where T : Component
    {
        public void Register(T component);
        public void Update(double deltaTime);
    }
}
