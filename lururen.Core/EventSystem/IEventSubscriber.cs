namespace lururen.EventSystem
{
    internal interface IEventSubscriber
    {
        public void OnEvent(EventArgs args);
    }
}