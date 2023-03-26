namespace Lururen.Client.EntityComponentSystem.Generic
{
    /// <summary>
    /// Base type for all visible components.
    /// </summary>
    public interface IRenderer : IComponent
    {
        /// <summary>
        /// Render event is bound to corresponding system render frame event.
        /// </summary>
        /// <param name="camera"></param>
        public void Render(Camera camera);
    }
}