using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lururen.Core.Common;

namespace Lururen.Core.EventSystem
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
            while (BufferedEvents.Any())
            {
                var evt = BufferedEvents.Pop();
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
            EventSubscribers[eventType].Remove(self);
        }
        public void Flush()
        {
            BufferedEvents.Clear();
        }
    }
}
