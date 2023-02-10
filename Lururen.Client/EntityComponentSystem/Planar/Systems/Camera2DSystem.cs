using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Planar.Systems
{
    public class Camera2DSystem : ISystem<Camera2D>
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
