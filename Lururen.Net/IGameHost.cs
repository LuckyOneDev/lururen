namespace Lururen.Net
{
    public interface IGameHost
    {
        public IGameData OnConnect(IClient client);
        public IGameData OnDisonnect(IClient client);
        public List<IClient> GetClients();
    }
}