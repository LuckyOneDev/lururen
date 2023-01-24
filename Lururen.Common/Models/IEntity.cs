using Lururen.Common.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Common.Models
{
    public interface IEntity : IDisposable, IEventSubscriber
    {

    }
}
