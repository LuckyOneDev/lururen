namespace Lururen.Net
{
    public interface IScript : IResource
    {
        public new ResourceType Type => ResourceType.CScript;
        public void Execute(IClient client);
    }
}