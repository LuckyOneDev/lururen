using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.EntityComponentSystem.Generic;
using System.Reflection;

namespace Lururen.Client.Base
{
    public class Prefab
    {
        public delegate void ConfigurationDelegate(Entity ent);
        public ConfigurationDelegate Configurator { get; }

        public Prefab(ConfigurationDelegate configurator)
        {
            this.Configurator = configurator;
        }
    }
}