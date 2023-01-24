using Lururen.Common.EventSystem;
using Lururen.Common.Extensions;

namespace Lururen.Server.EventSystem
{
    public class EventBus
    {
        public Stack<IEvent> BufferedEvents { get; set; } = new();
        public Dictionary<Type, List<IEventSubscriber>> EventSubscribers { get; set; } = new();

        public virtual void Init()
        {

        }
        public void ProcessEvents()
        {
            while (BufferedEvents.TryPop(out IEvent? evt))
            {
                if (EventSubscribers.TryGetValue(evt.GetType(), out var subscribers))
                {
                    subscribers.ForEach(subscriber =>
                    {
                        subscriber.OnEvent(evt.GetArgs());
                    });
                }
            }
        }
        public void PushEvent(IEvent evt)
        {
            BufferedEvents.Push(evt);
        }

        public void Subscribe(Type eventType, IEventSubscriber self)
        {
            EventSubscribers.AddOrCreateList(eventType, self);
        }

        public void Unsubscribe(IEventSubscriber self, Type eventType)
        {
            if (EventSubscribers.TryGetValue(eventType, out var result))
            {
                result.Remove(self);
            }
        }
        public void Flush()
        {
            BufferedEvents.Clear();
        }
    }
}
