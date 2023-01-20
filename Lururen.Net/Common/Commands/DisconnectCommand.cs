using Lururen.Core.App;
using Lururen.Core.CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.Common.Commands
{
    public class DisconnectCommand : ICommand
    {
        public void Run(Guid client, Application app)
        {
        }
    }
}
