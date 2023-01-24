using Lururen.Common.CommandSystem;
using Lururen.Server.Core.App;
using Lururen.Server.Core.CommandSystem;

namespace Lururen.Server.Networking.Commands
{
    public class DisconnectCommand : DisconnectCommandDTO, IRunnableCommand
    {
        public void Run(Guid client, Application app)
        {
        }
    }
}
