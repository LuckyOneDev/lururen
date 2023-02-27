﻿using OpenTK.Windowing.GraphicsLibraryFramework;
using ResourceLocation = Lururen.Client.ResourceManagement.ResourceLocation;
using Lururen.Client.Graphics.Texturing;
using Lururen.Client.Window;
using Lururen.Client.EntityComponentSystem.Planar;
using Lururen.Client.EntityComponentSystem;
using Lururen.Client.Base;
using System.Reflection.Metadata.Ecma335;
using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace GraphicsTestApp
{
    public static class PrefabCollection
    {
        public static Prefab Player => new Prefab() { Components = new() 
        {
            new() 
            { 
                TargetType = typeof(SpriteRenderer),
                FieldValues = new()
                {
                    { (typeof(SpriteRenderer).GetField(nameof(SpriteRenderer.Texture)), new Texture2D("GraphicsTestApp.megumin.png", ResourceLocation.Embeded)) },
                }
            },
            new()
            {
                TargetType = typeof(Camera2D)
            }
        } };
    }

    public class TestClient : Application2D
    {
        private const float camSpeed = 1000f;

        protected override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            if (InputManager.IsKeyDown(Keys.Escape))
            {
                this.Window.Close();
            }

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
                            //var rand = new Random();
                            //var texture = new Texture2D("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded);
                            ////var ent = new ImageEntity(texture);
                            //ent.Transform.Position.X = i * texture.Width;
                            //ent.Transform.Position.Y = j * texture.Height;
                            //ents.Enqueue(ent);
                        }
                    }
                    
                }

                // Test object disposal
                if (InputManager.IsKeyPressed(Keys.F))
                {
                    //var rand = new Random();
                    //var texture = new Texture2D("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded);
                    //var ent = new ImageEntity(texture);
                    //ent.Transform.Position.X = player.Transform.Position.X;
                    //ent.Transform.Position.Y = player.Transform.Position.Y;
                    //ent.Transform.Scale = 1f / rand.Next(1, 10);
                    //ent.Transform.Rotation = rand.Next(0, 360) / 360f;
                    //ents.Enqueue(ent);
                }
                    
                if (InputManager.IsKeyPressed(Keys.R))
                {
                    ents.TryDequeue(out Entity2D ent);
                    if (ent is not null) ent.Dispose();
                }
            }
        }

        private Queue<Entity2D> ents = new();
        private Entity2D player;

        protected override void Init()
        {
            base.Init();

            player = PrefabCollection.Player.Instantiate();
        }
    }
}