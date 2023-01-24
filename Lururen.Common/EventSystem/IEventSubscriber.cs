namespace Lururen.Common.EventSystem
{
    public interface IEventSubscriber
    {
        public void OnEvent(EventArgs args);
    }
}