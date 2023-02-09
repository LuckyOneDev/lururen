namespace Lururen.Client.EntityComponentSystem.Planar.Components
{
    public interface IRenderer : IComponent
    {
        public void Render(Camera camera);
    }
}