using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lururen.Common;
using lururen.EntitySystem;

namespace lururen.EnviromentSystem
{
    internal abstract class Enviroment : IDisposable
    {
        Dictionary<ScalarPoint, List<Entity>> Entities;
        List<Entity> UpdatableEntities;

        public void Init()
        {
            Entities = new Dictionary<ScalarPoint, List<Entity>>();
            UpdatableEntities = new List<Entity>();
        }
        public void Update()
        {
            UpdatableEntities.ForEach(entity => entity.Update());
        }
        public void Dispose()
        {
            Entities.Clear();
        }

        public List<Entity> SearchInRadius(ScalarPoint point, int radius)
        {
            throw new NotImplementedException();
        }

        public void AddEntity(Entity entity, ScalarPoint position, bool updatable = false)
        {
            if (Entities[position] != null)
            {
                Entities[position].Add(entity);
            }
            else
            {
                Entities[position] = new List<Entity>() { entity };
            }

            if (updatable)
            {
                UpdatableEntities.Add(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            //Entities.Remove(entity);
            throw new NotImplementedException();
        }
    }
}
