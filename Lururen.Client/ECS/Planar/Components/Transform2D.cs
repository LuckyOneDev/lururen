
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Planar.Components
{
    public class Transform2D : Component
    {
        
        public Transform2D(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
        }

        public float X { get; set;}
        public float Y { get; set; }

        public float Rotation { get; set; } = 0;
    }
}
