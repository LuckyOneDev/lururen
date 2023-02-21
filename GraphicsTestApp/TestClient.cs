using OpenTK.Windowing.GraphicsLibraryFramework;
using ResourceLocation = Lururen.Client.ResourceManagement.ResourceLocation;
using Lururen.Client.Graphics.Texturing;
using Lururen.Client.Window;
using Lururen.Client.EntityComponentSystem.Planar;
using Lururen.Client.EntityComponentSystem.Planar.Components;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.Audio.Generic;
using Lururen.Client.Audio;

namespace GraphicsTestApp
{
    public class ImageEntity : Entity2D
    {
        public ImageEntity(Texture2D texture)
        {
            AddComponent(new SpriteRenderer(this, texture));
        }
    }

    public class PlayerEntity : Entity2D
    {
        public PlayerEntity() : base()
        {
            var texture = new Texture2D("GraphicsTestApp.megumin.png", ResourceLocation.Embeded);
            var spriteRenderer = new SpriteRenderer(this, texture);
            AddComponent(spriteRenderer);
            AddComponent(new Camera2D(this));
            AddComponent(new SoundSource(this));

            spriteRenderer.Pivot = new OpenTK.Mathematics.Vector2(0.5f, 0.5f);
        }

        public async override void Init()
        {
            SoundSource ss = this.GetComponent<SoundSource>();

            _ = ss.Play(
                new Sound("GraphicsTestApp.npc_wolf_attackpower_01.wav", ResourceLocation.Embeded),
                new SoundPlayProperties()
                {
                    Looping = true
                });
        }
    }

    public class TestClient : ClientApp
    {
        private const float camSpeed = 1000f;

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
                if (InputManager.IsKeyDown(Keys.Q))
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        for (int j = 0; j < 1000; j++)
                        {
                            var rand = new Random();
                            var texture = new Texture2D("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded);
                            var ent = new ImageEntity(texture);
                            ent.Transform.Position.X = i * texture.Width;
                            ent.Transform.Position.Y = j * texture.Height;
                            ents.Enqueue(ent);
                        }
                    }
                    
                }

                // Test object disposal
                if (InputManager.IsKeyPressed(Keys.F))
                {
                    var rand = new Random();
                    var texture = new Texture2D("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded);
                    var ent = new ImageEntity(texture);
                    ent.Transform.Position.X = player.Transform.Position.X;
                    ent.Transform.Position.Y = player.Transform.Position.Y;
                    ent.Transform.Scale = 1f / rand.Next(1, 10);
                    ent.Transform.Rotation = rand.Next(0, 360) / 360f;
                    ents.Enqueue(ent);
                }
                    
                if (InputManager.IsKeyPressed(Keys.R))
                {
                    ents.TryDequeue(out Entity2D ent);
                    if (ent is not null) ent.Dispose();
                }
            }
        }

        private Queue<Entity2D> ents = new();
        private PlayerEntity player;

        public override void Init()
        {
            base.Init();
            player = new PlayerEntity();
        }

        public override void Render(double deltaTime)
        {
            base.Render(deltaTime);
        }
    }
}