using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Systems
{
    /// <summary>
    /// Handles cameras.
    /// </summary>
    public class CameraSystem : ISystem<Camera>
    {
        public List<Camera> Cameras = new();
        public void Register(Camera component)
        {
            Cameras.Add(component);
        }

        public void Update(double deltaTime)
        {
            Cameras.ForEach(camera => camera.Update(deltaTime));
        }

        public void Unregister(Camera component)
        {
            Cameras.Remove(component);
        }
    }
}
