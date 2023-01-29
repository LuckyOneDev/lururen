namespace Lururen.Client.ECS
{
    public class Entity
    {
        public Guid Id { get; set; }

        List<IComponent> Components = new List<IComponent>();

        public IComponent AddComponent(IComponent component)
        {
            component.Entity = this;
            Components.Add(component);
            return component;
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)Components.Find(component => component.GetType().Equals(typeof(T)));
        }

        public List<IComponent> GetComponents<T>() where T : IComponent
        {
            return Components.FindAll(component => component.GetType().Equals(typeof(T)));
        }
    }
}