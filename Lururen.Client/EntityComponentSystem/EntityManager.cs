namespace Lururen.Client.EntityComponentSystem
{
    /// <summary>
    /// Provides general purpose entity list.
    /// Every Entity is registered and unregistered automatically.
    /// </summary>
    public class EntityManager
    {
        protected Dictionary<Guid, Entity> Entities { get; set; } = new();

        #region Singleton

        private static EntityManager instance;

        private EntityManager()
        { }

        public static EntityManager GetInstance()
        {
            instance ??= new EntityManager();
            return instance;
        }

        #endregion Singleton

        /// <summary>
        /// Retrives entity with given type. If none is present returns default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetEntityByType<T>() where T : Entity => (T)Entities.Values.FirstOrDefault(x => x.GetType() == typeof(T));

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
    }
}