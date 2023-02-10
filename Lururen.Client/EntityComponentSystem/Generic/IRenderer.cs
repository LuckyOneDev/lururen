using Lururen.Client.EntityComponentSystem.Planar.Components;

namespace Lururen.Client.EntityComponentSystem.Generic
{
    public interface IRenderer : IComponent
    {
        public void Render(Camera2D camera);
    }
}