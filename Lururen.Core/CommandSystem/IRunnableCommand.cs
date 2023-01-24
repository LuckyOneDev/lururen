using Lururen.Common.CommandSystem;
using Lururen.Server.App;

namespace Lururen.Server.CommandSystem
{
    /// <summary>
    /// Command is requested action that is performed inside engine.
    /// </summary>
    public interface IRunnableCommand : ICommand
    {
        public void Run(Guid client, Application app);
    }
}
