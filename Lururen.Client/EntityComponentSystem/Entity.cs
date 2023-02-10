﻿using Lururen.Client.EntityComponentSystem.Generic;

namespace Lururen.Client.EntityComponentSystem
{
    /// <summary>
    /// Default implementation of IEntity<Component> interface.
    /// </summary>
    public class Entity : IEntity<Component>
    {

        private bool active = false;

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
            EntityManager.GetInstance().AddEntity(this);
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        List<Component> Components { get; set; } = new List<Component>();

        public Component AddComponent(Component component)
        {
            component.Init(this);
            Components.Add(component);
            component.Active = Active;
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