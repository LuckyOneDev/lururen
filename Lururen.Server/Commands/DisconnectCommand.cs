using Lururen.Common.CommandSystem;
using Lururen.Server.App;
using Lururen.Server.CommandSystem;

namespace Lururen.Server.Commands
{
    public class DisconnectCommand : DisconnectCommandDTO, IRunnableCommand
    {
        public void Run(Guid client, Application app)
        {
        }
    }
}
