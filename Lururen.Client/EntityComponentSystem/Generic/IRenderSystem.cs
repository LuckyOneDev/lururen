using Lururen.Client.Graphics;

namespace Lururen.Client.EntityComponentSystem.Generic
{
    /// <summary>
    /// Base type for any system that handles Renderers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRenderSystem<T> : ISystem<T> where T : IRenderer
    {
        /// <summary>
        /// Initializes render system and prepares it for drawing.
        /// </summary>
        /// <param name="window"></param>
        void Init(Game window);
    }
}