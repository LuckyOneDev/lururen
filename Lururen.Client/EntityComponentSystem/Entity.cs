using Lururen.Client.EntityComponentSystem.Generic;

namespace Lururen.Client.EntityComponentSystem
{
    /// <summary>
    /// Default implementation of IEntity<Component> interface.
    /// </summary>
    public class Entity : IEntity<Component>
    {

        private bool active;

        /// <summary>
        /// Gets or sets Active state of component.
        /// Also sets all child components equal to this value.
        /// </summary>
        public bool Active
        {
            get { return active; }
            set 
            { 
                active = value;
                Components.ForEach(x => x.Active = active);
            }
        }

        public Entity()
        {
            EntityComponentManager.GetInstance().AddEntity(this);
            Active = true;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        List<Component> Components { get; set; } = new List<Component>();

        public T AddComponent<T>(T component) where T : Component
        {
            component.Init();
            Components.Add(component);
            component.Active = Active;
            return component;
        }

        public virtual void Dispose()
        {
            Components.ForEach(x => x.Dispose());
            EntityComponentManager.GetInstance().RemoveEntity(this);
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

        public async virtual void Init()
        {

        }
    }
}