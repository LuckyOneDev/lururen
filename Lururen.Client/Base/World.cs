﻿using Lururen.Client.EntityComponentSystem.Base;

namespace Lururen.Client.Base
{
    public class World
    {
        public string Id { get; set; }
        public World(Application app, string wId)
        {
            Id = wId;
            this.Application = app;
        }

        public Application Application { get; }

        public Entity CreateEntity(Prefab prefab)
        {
            var entity = new Entity(this);
            prefab.Configurator(entity);
            return entity;
        }
    }
}