using lururen.EventSystem;

namespace lururen.EntitySystem
{
    /// <summary>
    /// Entity is any object that should contain data and/or logic of in game objects.
    /// E.g. characters, buttons, tiles.
    /// </summary>
    public abstract class Entity : IDisposable, IEventSubscriber
    {
        public abstract void Init();
        public abstract void Update();
        public abstract void Dispose();
        public abstract void OnEvent(EventArgs args);
        public List<IEvent> SubscribedEvents { get; set; } = new();
        public IEntityController? Controller { get; set; }
    }
}