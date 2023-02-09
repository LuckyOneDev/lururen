using OpenTK.Mathematics;

namespace Lururen.Client.EntityComponentSystem.Planar.Components
{
    public class Transform2D : Component
    {
        public Transform2D(Entity entity, float x = 0, float y = 0) : base(entity)
        {
            Position = new Vector2(x, y);
        }

        public Vector2 Position;
        public int Layer = 0;

        public float Scale = 1.0f;
        /// <summary>
        /// Rotation in radians
        /// </summary>
        public float Rotation = 0;
    }
}
