using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lururen.Core.App;
using Lururen.Core.Common;
using Lururen.Core.EntitySystem;

namespace Lururen.Core.EnviromentSystem
{
    public class Environment : IDisposable
    {
        public Application Application { get; private set; }
        private Dictionary<SVector3, List<Entity>> PassiveEntities { get; set; } = new();
        private Dictionary<SVector3, List<Entity>> ActiveEntities { get; set; } = new();
        public Environment(Application application)
        {
            Application = application;
        }

        public virtual void Init()
        {
            PassiveEntities.Values.SelectMany(x => x).AsParallel().ForAll(entity => entity.SysInit(Application));
            ActiveEntities.Values.SelectMany(x => x).AsParallel().ForAll(entity => entity.SysInit(Application));
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

        public List<Entity> SearchInRadius(SVector3 point, int radius)
        {
            List<Entity> result = new List<Entity>();
            for (int x = point.X - radius; x < point.X + radius; x++)
            {
                for (int y = point.Y - radius; y < point.Y + radius; y++)
                {
                    for (int z = point.Z - radius; z < point.Z + radius; z++)
                    {
                        SVector3 current = new SVector3(x, y, z);
                        if (ActiveEntities.ContainsKey(current))
                        {
                            result.AddRange(ActiveEntities[current]);
                        }

                        if (PassiveEntities.ContainsKey(current))
                        {
                            result.AddRange(PassiveEntities[current]);
                        }
                    }
                }
            }
            return result;
        }

        public void AddEntity(Entity entity, SVector3 position = new SVector3(), bool Active = false)
        {
            if (Active)
            {
                ActiveEntities.AddOrCreateList(position, entity);
            } else
            {
                PassiveEntities.AddOrCreateList(position, entity);
            }
        }

        public void AddEntityPassive(Entity entity, SVector3 position)
        {
            PassiveEntities.AddOrCreateList(position, entity);
        }
        public void AddEntityActive(Entity entity, SVector3 position)
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

        public void ActivateEntity(Entity entity)
        {
            PassiveEntities.MoveValueToOther(ActiveEntities, entity);
        }

        public void DeactivateEntity(Entity entity)
        {
            ActiveEntities.MoveValueToOther(PassiveEntities, entity);
        }

        public bool IsEntityActive(Entity entity)
        {
            bool active = ActiveEntities.Values.Any(entList => entList.Contains(entity));
            if (active)
            {
                return true;
            } 
            else
            {
                bool exists = PassiveEntities.Values.Any(entList => entList.Contains(entity));
                if (exists)
                {
                    return false;
                } 
                else
                {
                    throw new Exception("Entity does not exist in this enviroment.");
                }
            }

        }
    }
}
