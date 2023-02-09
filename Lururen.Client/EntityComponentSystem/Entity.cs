namespace Lururen.Client.ECS
{
    public class Entity : IDisposable
    {
        public Entity() 
        {
            EntityManager.GetInstance().AddEntity(this);
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        List<Component> Components = new List<Component>();

        public Component AddComponent(Component component)
        {
            component.Init(this);
            Components.Add(component);
            return component;
        }

        public T? GetComponent<T>() where T : Component
        {
            return (T?)Components.Find(component => component.GetType().Equals(typeof(T)));
        }

        public List<T> GetComponents<T>() where T : Component
        {
            return Components.FindAll(component => component.GetType().Equals(typeof(T))).Select(x => (T)x).ToList();
        }

        public virtual void Dispose()
        {
            Components.ForEach(x => x.Dispose());
            EntityManager.GetInstance().RemoveEntity(this);
        }
    }
}