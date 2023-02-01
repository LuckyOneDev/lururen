using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Components;
using Lururen.Client.Graphics;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Systems
{
    public class Context2D : BaseSystem<Component>
    {
        protected static Game Window { get; set; }

        public void Init(Game window)
        {
            Window = window;
        }

        internal static Camera? GetActiveCamera()
        {
            return Components.Find(x => x.GetType() == typeof(Camera)) as Camera;
        }

        public static Vector2i WindowSize => Window.ClientSize;

    }
}
