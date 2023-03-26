using Lururen.Client.EntityComponentSystem.Base;

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