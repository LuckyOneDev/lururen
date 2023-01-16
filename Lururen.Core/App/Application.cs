﻿using Lururen.Core.CommandSystem;
using Lururen.Core.Common;
using Lururen.Core.EnviromentSystem;
using Lururen.Core.EventSystem;
using Lururen.Networking.Common;
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
            CommandQueue = new CommandQueue(this);
            EventBus = new EventBus();
            Environments = new List<Environment>();
        }
        public IDataBus DataBus { get; protected set; }
        public bool IsRunning => CancellationTokenSource != null;
        public CommandQueue CommandQueue { get; set; }
        public List<Environment> Environments { get; set; }
        public EventBus EventBus { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; set; }
        public System.Timers.Timer CancelTimer { get; private set; }

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
        /// Immidiatly processes everything and goes to next frame
        /// </summary>
        public void ProcessAll()
        {
            EventBus.ProcessEvents();
            CommandQueue.ProcessCommands();
            Environments.ForEach(env => env.Update());
        }

        public void Start(TimeSpan frameDelay)
        {
            Init();
            CancelTimer = ThreadHelper.PeriodicThread(ProcessAll, frameDelay);
            DataBus.OnCommand += CommandQueue.Push;
            DataBus.Start();
        }

        public void Start(int tps = 60)
        {
            Start(TimeSpan.FromMilliseconds(1000 / tps));
        }

        public void Stop()
        {
            CancelTimer.Stop();
        }
    }
}
