using Lururen.Common.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Common.EntitySystem
{
    public interface IEntity : IDisposable, IEventSubscriber
    {
        public void Init();
        public void Update(double deltaTimeMs);
        public List<IEvent> SubscribedEvents { get; set; }
        public IEntityController? Controller { get; set; }
    }
}
