namespace Lururen.Client.EntityComponentSystem.Base
{
    /// <summary>
    /// Provides general purpose entity and component list.
    /// Every Entity and Component is registered and unregistered automatically.
    /// </summary>
    public class EntityComponentManager
    {
        protected Dictionary<Guid, Entity> Entities { get; set; } = new();
        protected Dictionary<Guid, Component> Components { get; set; } = new();

        #region Singleton

        private static EntityComponentManager instance;

        private EntityComponentManager()
        { }

        public static EntityComponentManager GetInstance()
        {
            instance ??= new EntityComponentManager();
            return instance;
        }

        #endregion Singleton

        #region Entity

        /// <summary>
        /// Retrives entity with given type. If none is present returns default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Entity? GetEntityById(Guid entId) => Entities.GetValueOrDefault(entId);

        /// <summary>
        /// Gets entity by its id.
        /// </summary>
        /// <typeparam name="T?"></typeparam>
        /// <param name="entityId"></param>
        /// <returns>null if entity is not present.</returns>
        public T? GetEntity<T>(Guid entityId) where T : Entity
        {
            return Entities[entityId] as T;
        }

        /// <summary>
        /// Adds entity to collection.
        /// Internal use only.
        /// </summary>
        /// <param name="ent"></param>
        internal void AddEntity(Entity ent)
        {
            Entities.Add(ent.Id, ent);
        }

        /// <summary>
        /// Removes entity from collection.
        /// Internal use only.
        /// </summary>
        /// <param name="ent"></param>
        internal void RemoveEntity(Entity ent)
        {
            Entities.Remove(ent.Id);
        }

        #endregion

        #region Component

        /// <summary>
        /// Retrives component with given type. If none is present returns default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetComponentByType<T>() where T : Component => (T?)Components.Values.FirstOrDefault(x => x.GetType() == typeof(T));

        /// <summary>
        /// Gets component by its id.
        /// </summary>
        /// <typeparam name="T?"></typeparam>
        /// <param name="componentID"></param>
        /// <returns>null if entity is not present.</returns>
        public T? GetComponent<T>(Guid componentID) where T : Component
        {
            return Components[componentID] as T;
        }

        /// <summary>
        /// Adds component to collection.
        /// Internal use only.
        /// </summary>
        /// <param name="comp"></param>
        internal void AddComponent(Component comp)
        {
            Components.Add(comp.Id, comp);
        }

        /// <summary>
        /// Removes component from collection.
        /// Internal use only.
        /// </summary>
        /// <param name="ent"></param>
        internal void RemoveComponent(Component ent)
        {
            Components.Remove(ent.Id);
        }

        #endregion
    }
}