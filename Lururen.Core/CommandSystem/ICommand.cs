using Lururen.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Core.CommandSystem
{
    /// <summary>
    /// Command is requested action that is performed inside engine.
    /// </summary>
    public interface ICommand
    {
        public void Run(Guid client, Application app);
    }
}
