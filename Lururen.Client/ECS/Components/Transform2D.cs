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
        public Transform2D()
        {
        }

        public Vector2 Position { get; set; }
    }
}
