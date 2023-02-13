using System.Collections.ObjectModel;

namespace Lururen.Common.ResourceManagement
{
    /// <summary>
    /// Base class for all resource handles.
    /// Guarantees that only one instance of resource is created.
    /// </summary>
    /// <typeparam name="Value"></typeparam>
    /// <typeparam name="Accessor"></typeparam>
    public abstract class ResourceHandle<Value, Accessor> where Accessor : notnull
    {
        /// <summary>
        /// Gets resource from memory or loads it.
        /// </summary>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public Value Get(Accessor accessor)
        {
            var loaded = IsLoaded(accessor);
            if (!loaded)
            {
                LoadResource(accessor);
            }
            return GetResource(accessor);
        }

        /// <summary>
        /// Returns all loaded resources.
        /// </summary>
        /// <returns></returns>
        public abstract ReadOnlyDictionary<Accessor, Value> GetLoadedResources();

        /// <summary>
        /// Determines if resource is loaded into memory.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsLoaded(Accessor acessor);

        /// <summary>
        /// Unloads resource from memory.
        /// </summary>
        /// <returns></returns>
        public abstract void UnloadResource(Accessor acessor);

        protected abstract Value GetResource(Accessor accessor);

        protected abstract void LoadResource(Accessor acessor);

    }
}