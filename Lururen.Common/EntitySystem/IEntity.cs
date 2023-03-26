using Lururen.Common.EventSystem;

namespace Lururen.Common.EntitySystem
{
    public interface IEntity : IDisposable, IEventSubscriber
    {
        public void Init();
        public void Update(double deltaTimeMs);
        public List<IEvent> SubscribedEvents { get; set; }
        public IEntityController? Controller { get; set; }
    }
}
