using Lururen.Client.EntityComponentSystem.Planar.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.EntityComponentSystem.Planar
{
    public class Entity2D : Entity
    {
        public Entity2D()
        {
            Transform = new Transform2D();
            AddComponent(Transform);
        }

        public Transform2D Transform { get; }
    }
}
