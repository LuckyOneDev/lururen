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
        public ImageEntity(Texture texture)
        {
            AddComponent(new Transform2D());
            AddComponent(new Sprite(texture));
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
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                this.Window.Close();
            }
        }

        public override void Init()
        {
            base.Init();

            var texture = new Texture(
                        ResourceHandle.Get("GraphicsTestApp.megumin.png", ResourceLocation.Embeded),
                        new Vector2(0.0f, 0.0f),
                        new Vector2(-0.5f, -0.5f)
            );

            EntityManager.AddEntity(new BaseCamera());
            EntityManager.AddEntity(new ImageEntity(texture));
        }
    }
}