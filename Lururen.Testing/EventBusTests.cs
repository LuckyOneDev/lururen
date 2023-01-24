using Lururen.Common.EventSystem;
using Lururen.Server.Core.EventSystem;

namespace Lururen.Testing
{
    public class EventBusTests
    {
        private class TestEvent : IEvent
        {
            public EventArgs GetArgs()
            {
                return EventArgs.Empty;
            }
        }

        private class TestEventSubscriber : IEventSubscriber
        {
            public bool EventHappened = false;

            public void OnEvent(EventArgs args)
            {
                EventHappened = true;
            }
        }

        private class ReccuringEventArgs : EventArgs
        {
            public bool wasCalled;
        }

        private class TestReccuringEvent : IEvent
        {
            public ReccuringEventArgs _args;

            public TestReccuringEvent(ReccuringEventArgs args)
            {
                _args = args;
            }

            public EventArgs GetArgs()
            {
                return _args;
            }
        }

        private class TestReccuringEventSubscriber : IEventSubscriber
        {
            public EventBus? eventBus;
            public bool RecursionHappened = false;

            public void OnEvent(EventArgs args)
            {
                if (args is ReccuringEventArgs recArgs)
                {
                    if (recArgs.wasCalled)
                    {
                        RecursionHappened = true;
                        return;
                    }
                    recArgs.wasCalled = true;
                    eventBus?.PushEvent(new TestReccuringEvent(recArgs));
                }
            }
        }

        [Fact]
        public void EventBusTest()
        {
            EventBus eventbus = new();
            TestEvent testEvent = new();
            TestEventSubscriber testSubscriber = new();

            eventbus.Subscribe(typeof(TestEvent), testSubscriber);
            eventbus.PushEvent(testEvent);

            Assert.False(testSubscriber.EventHappened);
            eventbus.ProcessEvents();
            Assert.True(testSubscriber.EventHappened);
        }

        [Fact]
        public void NoOneSubscribedToEventTest()
        {
            var eventbus = new EventBus();
            var testEvent = new TestReccuringEvent(new ReccuringEventArgs());
            var testSubscriber = new TestEventSubscriber();

            eventbus.Subscribe(typeof(TestEvent), testSubscriber);
            eventbus.PushEvent(testEvent);

            eventbus.ProcessEvents();
        }

        [Fact]
        public void EventRecursionTest()
        {
            var eventbus = new EventBus();
            var testEvent = new TestReccuringEvent(new ReccuringEventArgs());
            var testSubscriber = new TestReccuringEventSubscriber()
            {
                eventBus = eventbus
            };

            eventbus.Subscribe(typeof(TestReccuringEvent), testSubscriber);
            eventbus.PushEvent(testEvent);

            Assert.False(testSubscriber.RecursionHappened);
            eventbus.ProcessEvents();
            Assert.True(testSubscriber.RecursionHappened);
        }
    }
}