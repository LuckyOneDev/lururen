
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Planar.Components
{
    public class Transform2D : Component
    {
        public Transform2D(float x = 0, float y = 0)
        {
            Position= new Vector2(x, y);
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
