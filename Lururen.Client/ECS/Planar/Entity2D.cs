using Lururen.Client.ECS.Planar.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Planar
{
    public class Entity2D : Entity
    {
        public Entity2D() 
        {
            this.Transform = new Transform2D();
            AddComponent(this.Transform);
        }

        public Transform2D Transform { get; }
    }
}
