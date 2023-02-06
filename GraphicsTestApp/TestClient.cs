using Lururen.Client;
using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar;
using Lururen.Client.ECS.Planar.Components;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ResourceLocation = Lururen.Client.ResourceLocation;

namespace GraphicsTestApp
{
    public class ImageEntity : Entity2D
    {
        public ImageEntity(Texture2D texture)
        {
            AddComponent(new SpriteRenderer(texture));
        }
    }

    public class BaseCamera : Entity2D
    {
        public BaseCamera()
        {
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

            var texture = new Texture2D("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded);
            EntityManager.AddEntity(new BaseCamera());

            for (int j = 0; j < 200; j++)
            {
                for (int i = 0; i < 200; i++)
                {
                    var ent1 = new ImageEntity(texture);
                    ent1.Transform.Position.X = i * texture.Width;
                    ent1.Transform.Position.Y = j * texture.Height;
                }
            }

            //var ent1 = new ImageEntity(texture);
            //ent1.GetComponent<Transform2D>().Position.Y = 800;
            //ent1.GetComponent<Transform2D>().Position.X = 500;
            //EntityManager.AddEntity(ent1);
            //EntityManager.AddEntity(new ImageEntity(texture));
        }
    }
}