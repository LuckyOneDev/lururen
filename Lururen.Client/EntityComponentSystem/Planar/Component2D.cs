using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Planar
{
    /// <summary>
    /// Component whitch relies on Entity2D's Transform2D component.
    /// </summary>
    public abstract class Component2D : Component
    {
        /// <summary>
        /// Creates instance of Component2D.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="Exception"></exception>
        protected Component2D(Entity2D entity) : base(entity as Entity)
        {
            Transform = entity.GetComponent<Transform2D>()!;
        }

        public Transform2D Transform { get; set; }
    }
}