namespace Lururen.Server.Core.ResourceLoader
{
    /// <summary>
    /// Interface which provides methods to load
    /// Classes or Methods from external sources
    /// e.g. external dll or some script host
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// Loads a child of given class
        /// </summary>
        /// <typeparam name="T">A base type to return. Must have empty constructor.</typeparam>
        /// <param name="concreteChildName">A concrete class name to be loaded.</param>
        /// <returns>Base type instance of concrete class</returns>
        public T LoadChildOf<T>(string concreteChildName);

        /// <summary>
        /// Loads a given method
        /// </summary>
        /// <typeparam name="T">Method delegate type to load.</typeparam>
        /// <param name="methodName">A method name to load</param>
        /// <returns>Delegate to method</returns>
        public T LoadStaticMethod<T>(string methodName) where T : Delegate;
    }
}