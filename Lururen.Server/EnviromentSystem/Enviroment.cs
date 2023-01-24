using Lururen.Common.Extensions;
using Lururen.Common.Types;
using Lururen.Server.App;
using Lururen.Server.EntitySystem;

namespace Lururen.Server.EnviromentSystem
{
    public class Environment : IDisposable
    {
        public Application Application { get; private set; }
        private Dictionary<SVector3, List<ServerEntity>> PassiveEntities { get; set; } = new();
        private Dictionary<SVector3, List<ServerEntity>> ActiveEntities { get; set; } = new();
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

        public List<ServerEntity> SearchInRadius(SVector3 point, int radius)
        {
            List<ServerEntity> result = new();
            for (int x = point.X - radius; x < point.X + radius; x++)
            {
                for (int y = point.Y - radius; y < point.Y + radius; y++)
                {
                    for (int z = point.Z - radius; z < point.Z + radius; z++)
                    {
                        SVector3 current = new(x, y, z);
                        if (ActiveEntities.TryGetValue(current, out List<ServerEntity>? activeEnt))
                        {
                            result.AddRange(activeEnt);
                        }

                        if (PassiveEntities.TryGetValue(current, out List<ServerEntity>? passiveEnt))
                        {
                            result.AddRange(passiveEnt);
                        }
                    }
                }
            }
            return result;
        }

        public void AddEntity(ServerEntity entity, SVector3 position = new SVector3(), bool Active = false)
        {
            if (Active)
            {
                ActiveEntities.AddOrCreateList(position, entity);
            }
            else
            {
                PassiveEntities.AddOrCreateList(position, entity);
            }
        }

        public void AddEntityPassive(ServerEntity entity, SVector3 position)
        {
            PassiveEntities.AddOrCreateList(position, entity);
        }
        public void AddEntityActive(ServerEntity entity, SVector3 position)
        {
            ActiveEntities.AddOrCreateList(position, entity);
        }


        private void RemoveEntityPassive(ServerEntity entity)
        {
            ActiveEntities.RemoveFromList(entity);
        }

        private void RemoveEntityActive(ServerEntity entity)
        {
            PassiveEntities.RemoveFromList(entity);
        }
        public void RemoveEntity(ServerEntity entity)
        {
            RemoveEntityActive(entity);
            RemoveEntityPassive(entity);
        }

        public void ActivateEntity(ServerEntity entity)
        {
            PassiveEntities.MoveValueToOther(ActiveEntities, entity);
        }

        public void DeactivateEntity(ServerEntity entity)
        {
            ActiveEntities.MoveValueToOther(PassiveEntities, entity);
        }

        public bool IsEntityActive(ServerEntity entity)
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
