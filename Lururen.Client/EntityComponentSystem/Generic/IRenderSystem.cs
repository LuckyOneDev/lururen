using Lururen.Client.Graphics;

namespace Lururen.Client.EntityComponentSystem.Generic
{
    public interface IRenderSystem
    {
        /// <summary>
        /// Initializes render system and prepares it for drawing.
        /// </summary>
        /// <param name="window"></param>
        void Init(GLWindow window);

        /// <summary>
        /// Should be bound to corresponding update event.
        /// In base case it is called every frame.
        /// </summary>
        /// <param name="deltaTime">Time in seconds passed since last update</param>
        public void Update(double deltaTime);
    }

    /// <summary>
    /// Base type for any system that handles Renderers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRenderSystem<T> : IRenderSystem, ISystem<T> where T : IRenderer
    {
    }
}