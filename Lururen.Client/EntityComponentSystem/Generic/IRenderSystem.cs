using Lururen.Client.Graphics;

namespace Lururen.Client.EntityComponentSystem.Generic
{
    public interface IRenderSystem<T> : ISystem<T> where T : IRenderer
    {
        void Init(Game window);
    }
}