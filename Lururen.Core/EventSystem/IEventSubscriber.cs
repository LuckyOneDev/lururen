namespace Lururen.Core.EventSystem
{
    public interface IEventSubscriber
    {
        public void OnEvent(EventArgs args);
    }
}