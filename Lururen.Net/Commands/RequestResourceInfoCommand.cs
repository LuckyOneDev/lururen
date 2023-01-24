using Lururen.Common.CommandSystem;
using Lururen.Server.Core.App;
using Lururen.Server.Core.CommandSystem;

namespace Lururen.Server.Networking.Commands
{
    public class RequestResourceInfoCommand : RequestResourceInfoCommandDTO, IRunnableCommand
    {
        public void Run(Guid client, Application app)
        {
            var resourceInfo = app.GetResourceInfo();
            app.MessageBridge.SendData(client, resourceInfo);
        }
    }
}
