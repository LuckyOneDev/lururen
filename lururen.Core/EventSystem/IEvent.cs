namespace lururen.Core.EventSystem
{
    public interface IEvent
    {
        public EventArgs GetArgs();
    }
}