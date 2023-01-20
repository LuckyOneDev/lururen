using Lururen.Core.App;
using Lururen.Core.CommandSystem;

namespace Lururen.Networking.Common.Commands
{
    public class RequestResourceInfoCommand : ICommand
    {
        public void Run(Guid client, Application app)
        {
            var resourceInfo = app.GetResourceInfo();
            app.MessageBridge.SendData(client, resourceInfo);
        }
    }
}
