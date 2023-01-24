namespace Lururen.Common.EventSystem
{
    internal abstract class Event : IEvent
    {
        public abstract EventArgs GetArgs();
    }
}
