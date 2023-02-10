namespace Lururen.Client.EntityComponentSystem.Generic
{
    /// <summary>
    /// Base type for all systems.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISystem<T> where T : IComponent
    {
        /// <summary>
        /// Registers component in system and initializes it.
        /// </summary>
        /// <param name="component"></param>
        public void Register(T component);

        /// <summary>
        /// Removes component from system.
        /// </summary>
        /// <param name="component"></param>
        public void Unregister(T component);

        /// <summary>
        /// Should be bound to corresponding update event.
        /// In base case it is called every frame.
        /// </summary>
        /// <param name="deltaTime">Time in seconds passed since last update</param>
        public void Update(double deltaTime);
    }
}
