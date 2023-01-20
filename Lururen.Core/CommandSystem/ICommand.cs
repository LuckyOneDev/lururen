using Lururen.Core.App;

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
