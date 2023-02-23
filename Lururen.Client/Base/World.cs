using Lururen.Client.EntityComponentSystem;
using Lururen.Client.Window;

namespace Lururen.Client.Base
{
    public class World
    {
        public World(Application app)
        {
            this.Application = app;
        }

        public Application Application { get; }

        public Entity CreateEntity(Prefab prefab)
        {
            var entity = new Entity(this);
            prefab.Components.ForEach(component =>
            {
                entity.AddComponent(component);
            });
            return new Entity(this);
        }
    }
}