using Lururen.Core.EntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.Common
{
    public interface INetBus
    {
        public Task<IEnumerable<Entity>> SendMessage(IMessage message);
    }
}
