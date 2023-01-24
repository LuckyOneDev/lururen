using Lururen.Common.CommandSystem;
using Lururen.Common.Networking.Messages;
using Lururen.Server.App;
using Lururen.Server.CommandSystem;

namespace Lururen.Server.Commands
{
    public class RequestResourceCommand : RequestResourceCommandDTO, IRunnableCommand
    {
        public RequestResourceCommand(string resourceName) : base(resourceName)
        {
        }

        public void Run(Guid client, Application app)
        {
            var resourceStream = app.GetResource(ResourceName);
            app.MessageBridge.SendData(client, new FileTransmissionMessage(ResourceName, resourceStream.Length)).Wait();
            app.MessageBridge.SendContiniousData(client, resourceStream);
        }
    }
}