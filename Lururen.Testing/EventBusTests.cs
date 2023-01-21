namespace Lururen.Testing
{
    public class EventBusTests
    {
        class TestEvent : IEvent
        {
            public EventArgs GetArgs()
            {
                return EventArgs.Empty;
            }
        }

        class TestEventSubscriber : IEventSubscriber
        {
            public bool EventHappened = false;
            public void OnEvent(EventArgs args)
            {
                EventHappened = true;
            }
        }

        class ReccuringEventArgs : EventArgs
        {
            public bool wasCalled;
        }

        class TestReccuringEvent : IEvent
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

        class TestReccuringEventSubscriber : IEventSubscriber
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
