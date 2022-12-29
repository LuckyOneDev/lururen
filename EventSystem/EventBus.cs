using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lururen.EventSystem
{
    internal class EventBus
    {
        Stack<IEvent> BufferedEvents;
        Dictionary<IEvent, List<IEventSubscriber>> EventSubscribers;
        public void Init()
        {
            BufferedEvents = new();
            EventSubscribers = new();
        }
        public void ProcessEvents()
        {
            while (BufferedEvents.Any())
            {
                var evt = BufferedEvents.Pop();
                EventSubscribers[evt].ForEach(subscriber =>
                {
                    subscriber.OnEvent(evt.GetArgs());
                });
            }
        }
        public void PushEvent(IEvent evt)
        {
            BufferedEvents.Push(evt);
        }

        public void Subscribe(IEventSubscriber self, IEvent evt)
        {
            if (EventSubscribers[evt] != null)
            {
                EventSubscribers[evt].Add(self);
            }
            else
            {
                EventSubscribers[evt] = new List<IEventSubscriber>() { self };
            }
        }

        public void Unsubscribe(IEventSubscriber self, IEvent evt)
        {
            EventSubscribers[evt].Remove(self);
        }
    }
}
