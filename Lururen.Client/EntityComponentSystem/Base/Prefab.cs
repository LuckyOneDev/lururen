namespace Lururen.Client.EntityComponentSystem.Base
{
    public class Prefab
    {
        public delegate void ConfigurationDelegate(Entity ent);
        public ConfigurationDelegate Configurator { get; }

        public Prefab(ConfigurationDelegate configurator)
        {
            Configurator = configurator;
        }
    }
}