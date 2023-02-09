﻿namespace Lururen.Client.EntityComponentSystem
{
    public interface ISystem<T> where T : IComponent
    {
        public void Register(T component);
        public void Unregister(T component);
        public void Update(double deltaTime);
    }
}
