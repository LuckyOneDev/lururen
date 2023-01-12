namespace Lururen.Net
{
    public interface IGameData
    {
        public IScript InitScript { get; set; }
        public List<IScript> Scripts { get; set; }
        public List<IResource> Resources { get; set; }
    }
}