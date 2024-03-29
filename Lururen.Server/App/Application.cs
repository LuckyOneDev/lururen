﻿using Lururen.Common.Threading;
using Lururen.Common.Types;
using Lururen.Server.CommandSystem;
using Lururen.Server.EventSystem;
using Lururen.Server.Networking;
using Environment = Lururen.Server.EnviromentSystem.Environment;

namespace Lururen.Server.App
{
    /// <summary>
    /// This class contains static methods for looking up information about and controlling the run-time data.
    /// </summary>
    public abstract class Application
    {
        public Application(IServerMessageBridge messageBridge)
        {
            CommandQueue = new CommandQueue(this);
            EventBus = new EventBus();
            Environments = new List<Environment>();
            MessageBridge = messageBridge;
        }

        public IServerMessageBridge MessageBridge { get; }
        public bool IsRunning => CancelToken is not null;
        public CommandQueue CommandQueue { get; set; }
        public List<Environment> Environments { get; set; }
        public EventBus EventBus { get; set; }

        public CancellationTokenSource? CancelToken { get; private set; }

        public abstract void Dispose();

        public void Flush()
        {
            EventBus.Flush();
            CommandQueue.Flush();
        }

        public abstract void Init();
        public abstract ResourceInfo GetResourceInfo();

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
        public void ProcessAll(double deltaTime)
        {
            EventBus.ProcessEvents();
            CommandQueue.ProcessCommands();
            Environments.ForEach(env => env.Update(deltaTime));
        }

        public void Start(TimeSpan frameDelay)
        {
            Init();
            MessageBridge.OnCommand += CommandQueue.Push;
            CancelToken = ThreadHelper.StartPeriodicThread(ProcessAll, frameDelay);
            MessageBridge.Start();
        }

        public void Start(double tps = 60)
        {
            Start(TimeSpan.FromMilliseconds(1000d / tps));
        }

        public void Stop()
        {
            CancelToken?.Cancel();
        }

        public abstract Stream GetResource(string resourceName);
    }
}
