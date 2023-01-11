using Lururen.Core.CommandSystem;
using Lururen.Core.Common;
using Lururen.Core.EnviromentSystem;
using Lururen.Core.EventSystem;
using System;
using System.Threading;
using Environment = Lururen.Core.EnviromentSystem.Environment;

namespace Lururen.Core.App
{
    /// <summary>
    /// This class contains static methods for looking up information about and controlling the run-time data.
    /// </summary>
    public abstract class Application
    {
        public Application()
        {
            CommandQueue = new CommandQueue();
            EventBus = new EventBus();
            Environments = new List<EnviromentSystem.Environment>();
        }

        public CommandQueue CommandQueue { get; set; }
        public List<EnviromentSystem.Environment> Environments { get; set; }
        public EventBus EventBus { get; set; }
        private CancellationTokenSource CancellationToken { get; set; }

        public abstract void Dispose();

        public void Flush()
        {
            EventBus.Flush();
            CommandQueue.Flush();
        }

        public abstract void Init();

        public Environment CreateEnvironment()
        {
            return new Environment(this);
        }
        public void LoadEnviroment(Environment env)
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
        public void Start()
        {
            // Start eventbus and commandqueue
            CancellationToken = ThreadHelper.PeriodicThread(ProcessAll, TimeSpan.FromSeconds(1));
        }

        public void Stop()
        {
            CancellationToken.Cancel();
        }
    }
}
