using Lururen.Client.Graphics;

namespace Lururen.Client.EntityComponentSystem
{
    public interface IRenderSystem<T> : ISystem<T> where T : IRenderer
    {
        void Init(Game window);
    }
}