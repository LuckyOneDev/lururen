using Lururen.Client;
using OpenTK.Windowing.GraphicsLibraryFramework;

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
    }
}
