﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lururen.Core.Common;
using Lururen.Core.EntitySystem;
using Lururen.Core.Extensions;

namespace Lururen.Core.EnviromentSystem
{
    public abstract class Environment : IDisposable
    {
        private Dictionary<SVector3, List<Entity>> PassiveEntities { get; set; } = new();
        private Dictionary<SVector3, List<Entity>> ActiveEntities { get; set; } = new();

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

        public List<Entity> SearchInRadius(SVector3 point, int radius)
        {
            throw new NotImplementedException();
        }

        public void AddEntityPassive(SVector3 position, Entity entity)
        {
            PassiveEntities.AddOrCreateList(position, entity);
        }
        public void AddEntityActive(SVector3 position, Entity entity)
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


    }
}
