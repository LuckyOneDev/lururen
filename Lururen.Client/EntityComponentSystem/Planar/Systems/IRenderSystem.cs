using Lururen.Client.ECS.Planar.Components;
using Lururen.Client.Graphics;

namespace Lururen.Client.ECS.Planar.Systems
{
    public interface IRenderSystem<T> : ISystem<T> where T : IRenderer
    {
        void Init(Game window);
    }
}