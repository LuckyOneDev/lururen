using Lururen.Client.ECS;
using Lururen.Client.ECS.Planar.Components;
using Lururen.Client.Graphics.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ResourceLocation = Lururen.Client.ResourceManagement.ResourceLocation;
using Lururen.Client.Graphics.Texturing;
using Lururen.Client.Window;
using Lururen.Client.EntityComponentSystem.Planar;

#if DEBUG
using System.Diagnostics;
#endif

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
        public PlayerEntity() : base()
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


            if (InputManager.IsKeyDown(Keys.Escape))
            {
                this.Window.Close();
            }

            var player = EntityManager.GetEntityByType<PlayerEntity>();

            if (player is not null)
            {
                if (InputManager.IsKeyDown(Keys.Left))
                {
                    player.Transform.Position.X -= (float)deltaTime * camSpeed;
                }

                if (InputManager.IsKeyDown(Keys.Right))
                {
                    player.Transform.Position.X += (float)deltaTime * camSpeed;
                }

                if (InputManager.IsKeyDown(Keys.Up))
                {
                    player.Transform.Position.Y += (float)deltaTime * camSpeed;
                }

                if (InputManager.IsKeyDown(Keys.Down))
                {
                    player.Transform.Position.Y -= (float)deltaTime * camSpeed;
                }

                // Test object disposal
                if (InputManager.IsKeyDown(Keys.F))
                {
                    if (ents.Count != 0)
                    {
                        ents.ForEach(x => x.Dispose());
                        ents.Clear();
                    } 
                    else
                    {
                        var texture = new Texture2D("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded);
                        for (int j = 0; j < 100; j++)
                        {
                            for (int i = 0; i < 100; i++)
                            {
                                var ent = new ImageEntity(texture);
                                ent.Transform.Position.X = i * texture.Width;
                                ent.Transform.Position.Y = j * texture.Height;
                                ents.Add(ent);
                            }
                        }
                    }
                    
                }
            }

        }

        List<ImageEntity> ents = new();
        PlayerEntity player;
        public override void Init()
        {
            base.Init();
            player = new PlayerEntity();
        }

        public override void Render(double deltaTime)
        {
            base.Render(deltaTime);
            #if DEBUG
                Debug.WriteLine(1 / deltaTime); // fps
            #endif
        }
    }
}