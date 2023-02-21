using Lururen.Client.Graphics;

namespace Lururen.Client.EntityComponentSystem.Generic
{
    public interface ISoundSystem
    {
        void Init();
        public void Update(double deltaTime);
    }

    public interface ISoundSystem<T> : ISoundSystem, ISystem<T> where T : IComponent
    {

    }
}