using Lururen.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Core.CommandSystem
{
    public class CommandQueue
    {
        public CommandQueue(Application application) 
        {
            this.Application = application;
        }

        Application Application;
        private Queue<Tuple<Guid, ICommand>> Commands { get; } = new();
        public void Push(Guid caller, ICommand command)
        {
            Commands.Enqueue(new Tuple<Guid, ICommand>(caller, command));
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
