using Lururen.Client.ECS.Planar.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Planar.Systems
{
    public class CameraSystem : BaseSystem<Camera>
    {
        internal static Camera? GetActiveCamera()
        {
            return Components.Find(x => x.GetType() == typeof(Camera));
        }
    }
}
