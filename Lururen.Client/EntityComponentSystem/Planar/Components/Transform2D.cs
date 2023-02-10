using OpenTK.Mathematics;

namespace Lururen.Client.EntityComponentSystem.Planar.Components
{
    /// <summary>
    /// Represents position in 2D space.
    /// </summary>
    public class Transform2D : Component
    {
        public Transform2D(
            Entity entity,
            float posX = 0,
            float posY = 0,
            float rotation = 0,
            float scale = 0,
            int layer = 0) : base(entity)
        {
            Position = new Vector2(posX, posY);
            Scale = scale;
            Rotation = rotation;
            Layer = layer;
        }

        /// <summary>
        /// XY position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Layer on which parent component is. 
        /// More is closer to camera.
        /// </summary>
        public int Layer = 0;

        /// <summary>
        /// Scale in relative units.
        /// </summary>
        public float Scale = 1.0f;

        /// <summary>
        /// Rotation in radians.
        /// </summary>
        public float Rotation = 0;
    }
}
