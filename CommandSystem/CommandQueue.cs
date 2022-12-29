using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lururen.CommandSystem
{
    internal class CommandQueue
    {
        Queue<ICommand> commands = new Queue<ICommand>();
        public void Push(ICommand command)
        {
            commands.Enqueue(command);
        }

        public void Process()
        {
            while (commands.Count > 0)
            {
                var command = commands.Dequeue();
                command.Send();
            }

        }
    }
}
