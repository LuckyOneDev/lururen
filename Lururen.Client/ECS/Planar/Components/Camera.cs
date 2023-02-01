using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Systems;
using Lururen.Client.Graphics.Generic;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Components
{
    public class Camera : Component
    {
        public Camera() 
        {
            SpriteRenderer.Register(this);
        }

        public Transform2D Transform { get; private set; }
        public Vector2i ViewportSize { get; private set; }

        public void SetViewportSize(int width, int height)
        {
            ViewportSize = new Vector2i(width, height);
        }

        public void ReetViewportSize()
        {
            ViewportSize = SpriteRenderer.WindowSize;
        }

        public override void Init()
        {
            Transform = Entity.GetComponent<Transform2D>();
            ViewportSize = SpriteRenderer.WindowSize;
        }

        public override void Update(double deltaTime)
        {

        }
    }
}