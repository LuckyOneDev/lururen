using Lururen.Client;
using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Components;
using Lururen.Client.Graphics.Generic;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ResourceLocation = Lururen.Client.ResourceLocation;

namespace GraphicsTestApp
{
    public class ImageEntity : Entity
    {
        public ImageEntity(Texture2D texture)
        {
            AddComponent(new Transform2D());
            AddComponent(new SpriteRenderer(texture));
        }
    }

    public class BaseCamera : Entity
    {
        public BaseCamera()
        {
            AddComponent(new Transform2D());
            AddComponent(new Camera());
        }
    }

    public class TestClient : ClientApp
    {
        const float camSpeed = 100f;
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            Camera cam = Camera.GetActiveCamera();

            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                this.Window.Close();
            }

            if (cam is not null)
            {
                if (Keyboard.IsKeyDown(Keys.Left))
                {
                    cam.Transform.Position.X -= (float)deltaTime * camSpeed;
                }

                if (Keyboard.IsKeyDown(Keys.Right))
                {
                    cam.Transform.Position.X += (float)deltaTime * camSpeed;
                }

                if (Keyboard.IsKeyDown(Keys.Up))
                {
                    cam.Transform.Position.Y += (float)deltaTime * camSpeed;
                }

                if (Keyboard.IsKeyDown(Keys.Down))
                {
                    cam.Transform.Position.Y -= (float)deltaTime * camSpeed;
                }
            }

        }

        public override void Init()
        {
            base.Init();

            var texture = new Texture2D("GraphicsTestApp.megumin.png", ResourceLocation.Embeded);
            EntityManager.AddEntity(new BaseCamera());
            EntityManager.AddEntity(new ImageEntity(texture));
        }
    }
}