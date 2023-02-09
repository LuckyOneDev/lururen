namespace Lururen.Client.ECS.Planar.Components
{
    public interface IRenderer : IComponent
    {
        public void Render(Camera camera);
    }
}