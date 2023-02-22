﻿using Lururen.Client.Window;

namespace Lururen.Client.EntityComponentSystem.Generic
{
    public interface ISystem
    {
        /// <summary>
        /// Called once after registering system.
        /// </summary>
        public void Init(Application application) { }

        /// <summary>
        /// Should be bound to corresponding update event.
        /// In base case it is called every frame.
        /// </summary>
        /// <param name="deltaTime">Time in seconds passed since last update</param>
        public void Update(double deltaTime) { }

        /// <summary>
        /// Called once before destroying system.
        /// </summary>
        public void Destroy() { }
    }

    /// <summary>
    /// Base type for all systems.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISystem<T> : ISystem where T : IComponent<T>
    {
        /// <summary>
        /// Registers component in system and initializes it.
        /// </summary>
        /// <param name="component"></param>
        public void Register(T component);

        /// <summary>
        /// Removes component from system.
        /// </summary>
        /// <param name="component"></param>
        public void Unregister(T component);
    }
}
