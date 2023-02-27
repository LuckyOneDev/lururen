using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Components;
using static OpenTK.Compute.OpenCL.CLGL;

namespace Lururen.Client.EntityComponentSystem
{
    /// <summary>
    /// Default implementation of IEntity<Component> interface.
    /// </summary>
    public class Entity : IEntity<Component>
    {
        private bool Active { get; set; } = true;

        public void SetActive(bool state)
        {
            Active = state;
        }

        public bool IsActive()
        {
            return Active;
        }

        public Entity(World world)
        {
            EntityComponentManager.GetInstance().AddEntity(this);
            Transform = new Transform();
            this.World = world;
        }

        public Transform Transform { get; }

        public Guid Id { get; set; } = Guid.NewGuid();

        internal List<Component> Components { get; set; } = new List<Component>();
        public World World { get; }

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

        public T1 AddComponent<T1>() where T1 : Component
        {
            var component = (T1)Activator.CreateInstance(typeof(T1), this);
            Components.Add(component);
            return component;
        }

        public Component AddComponent(Type componentType)
        {
            var component = (Component)Activator.CreateInstance(componentType, this);
            Components.Add(component);
            return component;
        }
    }
}