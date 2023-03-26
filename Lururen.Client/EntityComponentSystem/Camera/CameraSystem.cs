using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Generic;

namespace Lururen.Client.EntityComponentSystem.Camera
{
    /// <summary>
    /// Handles cameras.
    /// </summary>
    public class CameraSystem : ISystem<Camera>
    {
        public List<Camera> Cameras = new();

        public Application Application { get; private set; }
        public void Init(Application app)
        {
            Application = app;
            Application.Window!.OnUpdate += Update;
        }

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

        /// <summary>
        /// Gets first active camera. Note that if there are more than one active camera
        /// it may return any of active cameras.
        /// </summary>
        /// <returns></returns>
        public Camera? GetActiveCamera()
        {
            // Check for active instead
            return Cameras.Find(x => x.IsActive() == true);
        }

        public void Destroy()
        {
        }
    }
}
