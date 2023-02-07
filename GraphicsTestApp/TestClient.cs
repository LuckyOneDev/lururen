using Lururen.Client;
using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar;
using Lururen.Client.ECS.Planar.Components;
using Lururen.Client.Graphics.Generic;
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

    public class PlayerEntity : Entity2D
    {
        public PlayerEntity()
        {
            var texture = new Texture2D("GraphicsTestApp.megumin.png", ResourceLocation.Embeded);
            var spriteRenderer = new SpriteRenderer(texture);
            AddComponent(spriteRenderer);
            AddComponent(new Camera());
            spriteRenderer.Pivot = new OpenTK.Mathematics.Vector2(0.5f, 0.5f);
        }
    }

    public class TestClient : ClientApp
    {
        const float camSpeed = 1000f;
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);


            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                this.Window.Close();
            }

            var player = EntityManager.GetEntityByType<PlayerEntity>();

            if (player is not null)
            {
                if (Keyboard.IsKeyDown(Keys.Left))
                {
                    player.Transform.Position.X -= (float)deltaTime * camSpeed;
                }

                if (Keyboard.IsKeyDown(Keys.Right))
                {
                    player.Transform.Position.X += (float)deltaTime * camSpeed;
                }

                if (Keyboard.IsKeyDown(Keys.Up))
                {
                    player.Transform.Position.Y += (float)deltaTime * camSpeed;
                }

                if (Keyboard.IsKeyDown(Keys.Down))
                {
                    player.Transform.Position.Y -= (float)deltaTime * camSpeed;
                }
            }

        }

        public override void Init()
        {
            base.Init();

            var texture = new Texture2D("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded);
            EntityManager.AddEntity(new PlayerEntity());

            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < 100; i++)
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