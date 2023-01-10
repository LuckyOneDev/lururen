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

        [Fact]
        public void EventBusTest()
        {
            var eventbus = new EventBus();
            var testEvent = new TestEvent();
            var testSubscriber = new TestEventSubscriber();

            eventbus.Subscribe(testEvent, testSubscriber);
            eventbus.PushEvent(testEvent);

            Assert.False(testSubscriber.EventHappened);
            eventbus.ProcessEvents();
            Assert.True(testSubscriber.EventHappened);
        }
    }
}
