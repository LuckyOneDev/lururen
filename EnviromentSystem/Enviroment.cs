using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lururen.Common;
using lururen.EntitySystem;
using lururen.Extensions;

namespace lururen.EnviromentSystem
{
    internal abstract class Enviroment : IDisposable
    {
        private Dictionary<ScalarPoint, List<Entity>> PassiveEntities { get; set; } = new();
        private Dictionary<ScalarPoint, List<Entity>> ActiveEntities { get; set; } = new();

        public virtual void Init()
        {
            // Add initialization logic here
        }
        public virtual void Update()
        {
            ActiveEntities.Values.SelectMany(x => x).AsParallel().ForAll(x => x.Update());
        }
        public virtual void Dispose()
        {
            ActiveEntities.Clear();
            PassiveEntities.Clear();
        }

        public List<Entity> SearchInRadius(ScalarPoint point, int radius)
        {
            throw new NotImplementedException();
        }

        public void AddEntityPassive(ScalarPoint position, Entity entity)
        {
            PassiveEntities.AddOrCreateList(position, entity);
        }
        public void AddEntityActive(ScalarPoint position, Entity entity)
        {
            ActiveEntities.AddOrCreateList(position, entity);
        }


        private void RemoveEntityPassive(Entity entity)
        {
            ActiveEntities.RemoveFromList(entity);
        }

        private void RemoveEntityActive(Entity entity)
        {
            PassiveEntities.RemoveFromList(entity);
        }
        public void RemoveEntity(Entity entity)
        {
            RemoveEntityActive(entity);
            RemoveEntityPassive(entity);
        }

        public void Activate(Entity entity)
        {
            PassiveEntities.MoveValueToOther(ActiveEntities, entity);
        }

        public void Deactivate(Entity entity)
        {
            ActiveEntities.MoveValueToOther(PassiveEntities, entity);
        }


    }
}
