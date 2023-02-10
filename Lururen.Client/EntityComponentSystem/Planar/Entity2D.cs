using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Planar
{
    /// <summary>
    /// Entity with guranteed Transform2D created.
    /// </summary>
    public class Entity2D : Entity
    {
        public Entity2D()
        {
            Transform = new Transform2D(this);
            AddComponent(Transform);
        }

        public Transform2D Transform { get; }
    }
}
