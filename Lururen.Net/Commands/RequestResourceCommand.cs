using Lururen.Common.CommandSystem;
using Lururen.Common.Networking.Messages;
using Lururen.Server.Core.App;
using Lururen.Server.Core.CommandSystem;

namespace Lururen.Server.Networking.Commands
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