using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Systems;
using Lururen.Client.Graphics.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Components
{
    public class Camera : Component
    {
        public Camera() 
        {
            CameraSystem.GetInstance().Register(this);
        }

        public Transform2D Transform { get; private set; }
        public Vector2i ViewportSize { get; private set; }

        public void SetViewportSize(int width, int height)
        {
            ViewportSize = new Vector2i(width, height);
        }

        public void ResetViewportSize()
        {
            ViewportSize = Renderer2D.WindowSize;
        }

        public override void Init(Entity ent)
        {
            base.Init(ent);
            Transform = Entity.GetComponent<Transform2D>();
            ViewportSize = Renderer2D.WindowSize;
        }

        public override void Update(double deltaTime)
        {
            ViewportSize = Renderer2D.WindowSize;
            GL.Viewport(0, 0, ViewportSize.X, ViewportSize.Y);
        }

        public static Camera? GetActiveCamera()
        {
            // Check for active instead
            return CameraSystem.GetInstance().Cameras.Find(x => x.GetType() == typeof(Camera));
        }

        internal Vector2 GetPositionCorrector()
        {
            return -Transform.Position + ViewportSize / 2;
        }

        internal float GetRotationCorrector()
        {
            return -Transform.Rotation;
        }
    }
}