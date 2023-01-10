using Lururen.Core.CommandSystem;
using Lururen.Core.Common;
using Lururen.Core.EnviromentSystem;
using Lururen.Core.EventSystem;
using System;
using System.Threading;

namespace Lururen.Core.App
{
    /// <summary>
    /// This class contains static methods for looking up information about and controlling the run-time data.
    /// </summary>
    public abstract class Application
    {
        public CommandQueue CommandQueue { get; set; }
        public EventBus EventBus { get; set; }
        public List<EnviromentSystem.Environment> Environments { get; set; }
        public Application() 
        {
            CommandQueue = new CommandQueue();
            EventBus = new EventBus();
            Environments = new List<EnviromentSystem.Environment>();
        }

        public void LoadEnviroment(EnviromentSystem.Environment env)
        {
            Environments.Add(env);
            env.Init();
        }

        /// <summary>
        /// Immidiatly processes everything
        /// </summary>
        public void ProcessAll()
        {
            EventBus.ProcessEvents();
            CommandQueue.ProcessCommands();
        }

        public void Flush()
        {
            EventBus.Flush();
            CommandQueue.Flush();
        }

        public void Start()
        {
            // Start eventbus and commandqueue
            var cancellationToken = ThreadHelper.PeriodicThread(ProcessAll, TimeSpan.FromSeconds(1));
        }

        public void Stop()
        {

        }

        public abstract void Init();
        public abstract void Dispose();
    }
}
