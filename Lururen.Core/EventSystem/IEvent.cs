namespace Lururen.Core.EventSystem
{
    public interface IEvent
    {
        public EventArgs GetArgs();
    }
}