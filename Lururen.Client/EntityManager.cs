using Lururen.Client.ECS;
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
        protected List<Entity> Entities { get; set; } = new();
        public EntityManager() { }

        public void AddEntity(Entity ent)
        {
            Entities.Add(ent);
        }
    }
}
