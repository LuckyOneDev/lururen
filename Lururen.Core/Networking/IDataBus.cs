using Lururen.Core.EntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.Common
{
    public interface IDataBus : IDisposable
    {
        bool Running { get; }
        public Task<IEnumerable<Entity>> OnMessage(IMessage command);
        Task Start();
        Task Stop();
    }
}
