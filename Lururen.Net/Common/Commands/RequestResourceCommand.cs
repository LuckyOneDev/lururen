using Lururen.Core.App;
using Lururen.Core.CommandSystem;

namespace Lururen.Networking.Common.Commands
{
    public class RequestResourceCommand : ICommand
    {
        public RequestResourceCommand(string resourceName)
        {
            ResourceName = resourceName;
        }
        public string ResourceName { get; }
        public void Run(Guid client, Application app)
        {
            var resourceStream = app.GetResource(ResourceName);
            app.DataBus.SendData(client, new FileTransmission(ResourceName, resourceStream.Length)).Wait();
            app.DataBus.SendContiniousData(client, resourceStream);
        }
    }
}