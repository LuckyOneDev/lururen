namespace Lururen.Client.EntityComponentSystem.Generic
{
    /// <summary>
    /// Basic type for all object that should be used in game.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<T> : IDisposable where T : IComponent
    {
        /// <summary>
        /// Unique identifier of this entity. Used for internal purposes and for quick search if needed.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Adds component to this Entity's component collection and 
        /// sets Component's Entity field.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public T1 AddComponent<T1>(T1 component) where T1 : T;

        /// <summary>
        /// Retrieves any component of given type.
        /// Should only be used if one instance of component is present on Entity.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public T1? GetComponent<T1>() where T1 : T;

        /// <summary>
        /// Retrieves list of components of given type. If no components is found List should be empty.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public List<T1> GetComponents<T1>() where T1 : T;

        /// <summary>
        /// Removes component from this entity's component collection and disposes it.
        /// </summary>
        /// <param name="component"></param>
        public void RemoveComponent(T component);
    }
}