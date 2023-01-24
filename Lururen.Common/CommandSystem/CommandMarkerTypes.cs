namespace Lururen.Common.CommandSystem
{
    public class DisconnectCommandDTO : ICommand
    {
    }

    public class RequestResourceCommandDTO : ICommand
    {
        public RequestResourceCommandDTO(string resourceName)
        {
            ResourceName = resourceName;
        }
        public string ResourceName { get; }
    }

    public class RequestResourceInfoCommandDTO : ICommand
    {
    }
}
