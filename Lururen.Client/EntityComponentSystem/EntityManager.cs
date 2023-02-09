namespace Lururen.Client.EntityComponentSystem
{
    public class EntityManager
    {
        protected Dictionary<Guid, Entity> Entities { get; set; } = new();

        #region Singleton
        private static EntityManager instance;

        private EntityManager() { }

        public static EntityManager GetInstance()
        {
            instance ??= new EntityManager();
            return instance;
        }
        #endregion

        public T GetEntityByType<T>() where T : Entity => (T)Entities.Values.FirstOrDefault(x => x.GetType() == typeof(T));

        public void AddEntity(Entity ent)
        {
            Entities.Add(ent.Id, ent);
        }

        public void RemoveEntity(Entity ent)
        {
            Entities.Remove(ent.Id);
        }
    }
}
