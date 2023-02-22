using Lururen.Client.EntityComponentSystem.Generic;

namespace Lururen.Client.EntityComponentSystem.Planar.Systems
{
    internal class SoundListener : IComponent<SoundListener>
    {
        public Entity? Entity { get; set; }
        public bool Active { get; set; }

        public void Init(ISystem<SoundListener> system)
        {
            system.Register(this);
        }

        public void Update(double deltaTime)
        {
        }

        public void Dispose()
        {
        }
    }
}