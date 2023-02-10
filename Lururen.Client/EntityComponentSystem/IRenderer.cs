using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem
{
    public interface IRenderer : IComponent
    {
        public void Render(Camera2D camera);
    }
}