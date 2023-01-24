using Lururen.Server.Core.App;
using System.Collections.Concurrent;

namespace Lururen.Server.Core.CommandSystem
{
    public class CommandQueue
    {
        public CommandQueue(Application application)
        {
            Application = application;
        }

        private readonly Application Application;
        private ConcurrentQueue<Tuple<Guid, IRunnableCommand>> Commands { get; } = new();
        public void Push(Guid caller, IRunnableCommand command)
        {
            var cmd = new Tuple<Guid, IRunnableCommand>(caller, command);
            Commands.Enqueue(cmd);
        }

        public void ProcessCommands()
        {
            while (Commands.TryDequeue(out var command))
            {
                command.Item2.Run(command.Item1, Application);
            }
        }

        public void Flush()
        {
            Commands.Clear();
        }
    }
}
