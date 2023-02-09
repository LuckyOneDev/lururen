using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Planar.Systems
{
    public class Camera2DSystem : ISystem<Camera>
    {
        #region Singleton
        private static Camera2DSystem instance;

        private Camera2DSystem() { }

        public static Camera2DSystem GetInstance()
        {
            if (instance == null)
                instance = new Camera2DSystem();
            return instance;
        }
        #endregion

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
