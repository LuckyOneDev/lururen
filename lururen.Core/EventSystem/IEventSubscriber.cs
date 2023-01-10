namespace Lururen.Core.EventSystem
{
    internal interface IEventSubscriber
    {
        public void OnEvent(EventArgs args);
    }
}