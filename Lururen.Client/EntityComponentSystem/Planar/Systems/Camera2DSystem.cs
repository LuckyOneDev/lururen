using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Planar.Systems
{
    /// <summary>
    /// Handles cameras.
    /// </summary>
    public class Camera2DSystem : ISystem<Camera2D>
    {
        public List<Camera2D> Cameras = new();
        public void Register(Camera2D component)
        {
            Cameras.Add(component);
        }

        public void Update(double deltaTime)
        {
            Cameras.ForEach(camera => camera.Update(deltaTime));
        }

        public void Unregister(Camera2D component)
        {
            Cameras.Remove(component);
        }
    }
}
