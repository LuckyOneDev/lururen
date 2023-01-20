namespace Lururen.Core.EventSystem
{
    internal abstract class Event : IEvent
    {
        public abstract EventArgs GetArgs();
    }
}
