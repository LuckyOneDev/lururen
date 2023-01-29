using Lururen.Common.EntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client
{
    public class EntityManager
    {
        public List<IEntity> Entities { get; set; } = new();
        public EntityManager() { }
        public virtual void Init()
        {
            Parallel.ForEach(Entities, (entity) => entity.Init());
        }
        public virtual void Update(double deltaTime)
        {
            Parallel.ForEach(Entities, (entity) => entity.Update(deltaTime));
        }
    }
}
