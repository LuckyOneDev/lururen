using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Components
{
    public class Transform2D : Component
    {
        public Transform2D(float x = 0, float y = 0)
        {
            Position= new Vector2(x, y);
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; } = 0;
    }
}
