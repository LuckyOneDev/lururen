using Lururen.Common.CommandSystem;
using Lururen.Server.Core.App;

namespace Lururen.Server.Core.CommandSystem
{
    /// <summary>
    /// Command is requested action that is performed inside engine.
    /// </summary>
    public interface IRunnableCommand : ICommand
    {
        public void Run(Guid client, Application app);
    }
}
