using Lururen.Common.CommandSystem;
using Lururen.Server.App;

namespace Lururen.Server.CommandSystem.Commands
{
    public class DisconnectCommand : DisconnectCommandDTO, IRunnableCommand
    {
        public void Run(Guid client, Application app)
        {
        }
    }
}
