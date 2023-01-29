using Lururen.Client;
using Lururen.Client.Graphics.Drawables;
using Lururen.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection;

namespace GraphicsTestApp
{
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
            //RenderingContext.AddElement(new Triangle(new Vector2(-0.5f, -0.5f),
            //                                         new Vector2(0.5f, -0.5f),
            //                                         new Vector2(0.0f, 0.5f),
            //                                         Color4.Red));

            RenderingContext.AddElement(new Texture2D(
                    ResourceHelper.GetEmbededResourceStream(Assembly.GetExecutingAssembly(), "GraphicsTestApp.wall.jpg"),
                    new Vector2(0.0f, 0.5f),
                    new Vector2(-0.5f, -0.5f)
            ));
        }
    }
}