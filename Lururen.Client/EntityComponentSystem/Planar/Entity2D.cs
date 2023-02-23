using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Planar
{
    /// <summary>
    /// Entity with guranteed Transform2D created.
    /// </summary>
    public sealed class Entity2D : Entity
    {
        public Entity2D(World world) : base(world)
        {
            Transform = AddComponent<Transform2D>();
        }

        public Transform2D Transform { get; }
    }
}
