namespace Lururen.Common.EventSystem
{
    public interface IEvent
    {
        public EventArgs GetArgs();
    }
}