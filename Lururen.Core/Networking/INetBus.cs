using Lururen.Core.EntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.Common
{
    public interface INetBus : IDisposable
    {
        public Task<IEnumerable<Entity>> SendMessage(IMessage message);
        Task Start();
        Task Stop();
    }
}
