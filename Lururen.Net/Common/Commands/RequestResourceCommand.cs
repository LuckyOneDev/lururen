using Lururen.Core.App;
using Lururen.Core.CommandSystem;
using Lururen.Networking.Common.ServerMessages;

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
            app.MessageBridge.SendData(client, new FileTransmission(ResourceName, resourceStream.Length)).Wait();
            app.MessageBridge.SendContiniousData(client, resourceStream);
        }
    }
}