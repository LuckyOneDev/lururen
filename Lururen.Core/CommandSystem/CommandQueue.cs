using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Core.CommandSystem
{
    public class CommandQueue
    {
        private Queue<ICommand> Commands { get; } = new();
        public void Push(ICommand command)
        {
            Commands.Enqueue(command);
        }

        public void ProcessCommands()
        {
            while (Commands.TryDequeue(out var command))
            {
                command.Run();
            }
        }

        public void Flush()
        {
            Commands.Clear();
        }
    }
}
