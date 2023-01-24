using Lururen.Common.CommandSystem;
using Lururen.Server.App;
using Lururen.Server.CommandSystem;

namespace Lururen.Server.Commands
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
