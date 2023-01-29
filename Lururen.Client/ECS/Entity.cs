namespace Lururen.Client.ECS
{
    public class Entity
    {
        public Guid Id { get; set; }

        List<Component> Components = new List<Component>();

        public Component AddComponent(Component component)
        {
            component.Entity = this;
            component.Init();
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
    }
}