using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Planar
{
    public abstract class Component2D : Component
    {
        protected Component2D(Entity entity) : base(entity)
        {
            if (entity is not Entity2D) throw new Exception();
            Transform = entity.GetComponent<Transform2D>()!;
        }

        public Transform2D Transform { get; set; }
    }
}