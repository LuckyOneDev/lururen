using Lururen.Core.EntitySystem;
using Lururen.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.LocalBus
{
    internal abstract class LocalNetDataBus : INetBus, IDataBus
    {
        public abstract Task<IEnumerable<Entity>> OnMessage(IMessage command);
        public Task<IEnumerable<Entity>> SendMessage(IMessage message)
        {
            return OnMessage(message);
        }

        public bool Running => true;

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
