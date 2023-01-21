using Lururen.Core.App;
using System.Collections.Concurrent;

namespace Lururen.Core.CommandSystem
{
    public class CommandQueue
    {
        public CommandQueue(Application application)
        {
            this.Application = application;
        }

        private readonly Application Application;
        private ConcurrentQueue<Tuple<Guid, ICommand>> Commands { get; } = new();
        public void Push(Guid caller, ICommand command)
        {
            var cmd = new Tuple<Guid, ICommand>(caller, command);
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
