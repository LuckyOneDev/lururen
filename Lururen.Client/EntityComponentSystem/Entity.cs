namespace Lururen.Client.EntityComponentSystem
{

    public class Entity : IEntity<Component>
    {
        public Entity()
        {
            EntityManager.GetInstance().AddEntity(this);
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        List<Component> Components { get; set; } = new List<Component>();

        public Component AddComponent(Component component)
        {
            component.Init(this);
            Components.Add(component);
            return component;
        }

        public virtual void Dispose()
        {
            Components.ForEach(x => x.Dispose());
            EntityManager.GetInstance().RemoveEntity(this);
        }

        public T1? GetComponent<T1>() where T1 : Component
        {
            return (T1?)Components.Find(component => component.GetType().Equals(typeof(T1)));
        }

        public List<T1> GetComponents<T1>() where T1 : Component
        {
            return Components.FindAll(component => component.GetType().Equals(typeof(T1))).Select(x => (T1)x).ToList();
        }

        public void RemoveComponent(Component component)
        {
            Components.Remove(component);
            component.Dispose();
        }
    }
}