using Lururen.Client;
using Lururen.Client.ECS;
using Lururen.Client.ECS.Components;
using Lururen.Client.ECS.Drawing2D;
using Lururen.Client.Graphics.Drawables;
using Lururen.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection;
using ResourceLocation = Lururen.Client.ResourceLocation;

namespace GraphicsTestApp
{
    public class ImageEntity : Entity
    {
        public ImageEntity(Texture2D texture)
        {
            AddComponent(new Transform2D());
            AddComponent(new Sprite(texture));
        }
    }

    public class TestClient : ClientApp
    {
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            if (KeyboardInputManager.IsKeyDown(Keys.Escape))
            {
                this.Window.Close();
            }
        }

        public override void Init()
        {
            base.Init();

            var texture = new Texture2D(
                        ResourceHandle.Get("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded),
                        new Vector2(0.0f, 0.0f),
                        new Vector2(-0.5f, -0.5f)
            );

            
            EntityManager.AddEntity(new ImageEntity(texture));
        }
    }
}