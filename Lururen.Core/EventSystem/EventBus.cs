using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lururen.Core.Extensions;

namespace Lururen.Core.EventSystem
{
    internal class EventBus
    {
        public Stack<IEvent> BufferedEvents { get; set; } = new();
        public Dictionary<IEvent, List<IEventSubscriber>> EventSubscribers { get; set; } = new();

        public virtual void Init()
        {

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

        public void Subscribe(IEvent evt, IEventSubscriber self)
        {
            EventSubscribers.AddOrCreateList(evt, self);
        }

        public void Unsubscribe(IEventSubscriber self, IEvent evt)
        {
            EventSubscribers[evt].Remove(self);
        }
    }
}
