using Lururen.Client.ECS.Planar;
using Lururen.Client.ECS.Planar.Components;

namespace Lururen.Client.ECS
{
    public abstract class Component2D : Component
    {
        public Transform2D Transform { get; set; }
        public override void Init(Entity entity)
        {
            if (entity is not Entity2D) throw new Exception();
            base.Init(entity);
            Transform = entity.GetComponent<Transform2D>()!;
        }
    }
}