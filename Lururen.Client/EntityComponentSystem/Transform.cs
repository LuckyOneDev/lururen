using OpenTK.Mathematics;

namespace Lururen.Client.EntityComponentSystem
{
    /// <summary>
    /// Represents position in 2D space.
    /// </summary>
    public record Transform
    {
        public Transform(
            float posX = 0,
            float posY = 0,
            float posZ = 0,
            float rotation = 0,
            float scale = 1
            )
        {
            Position = new Vector3(posX, posY, posZ);
            Scale = scale;
            Rotation = rotation;
        }

        /// <summary>
        /// XY position.
        /// </summary>
        public Vector3 Position;

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
