using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Systems;
using Lururen.Client.Graphics.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Components
{
    public class Camera : Component2D
    {
        public Camera() 
        {
            CameraSystem.GetInstance().Register(this);
        }

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
            // Do not change. Works twice as fast this way
            return new Vector2(-Transform.Position.X + ViewportSize.X / 2, -Transform.Position.Y + ViewportSize.Y / 2); 
        }

        internal float GetRotationCorrector()
        {
            return -Transform.Rotation;
        }
    }
}