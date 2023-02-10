namespace Lururen.Client.EntityComponentSystem.Generic
{
    public interface IEntity<T> : IDisposable where T : IComponent
    {
        public Guid Id { get; set; }
        public T AddComponent(T component);
        public T1? GetComponent<T1>() where T1 : T;
        public List<T1> GetComponents<T1>() where T1 : T;
        public void RemoveComponent(T component);
    }
}