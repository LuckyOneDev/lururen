using Lururen.Client.ECS.Planar.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Planar.Systems
{
    public class CameraSystem : ISystem<Camera>
    {
        #region Singleton
        private static CameraSystem instance;

        private CameraSystem() { }

        public static CameraSystem GetInstance()
        {
            if (instance == null)
                instance = new CameraSystem();
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
