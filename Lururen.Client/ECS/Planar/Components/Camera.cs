using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Systems;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Components
{
    public class Camera : Component
    {
        public Camera() 
        {
            Context2D.Register(this);
        }
    }
}