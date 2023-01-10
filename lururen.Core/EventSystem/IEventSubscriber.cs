namespace lururen.Core.EventSystem
{
    internal interface IEventSubscriber
    {
        public void OnEvent(EventArgs args);
    }
}