using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.Window;

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
            prefab.Components.ForEach(component =>
            {
                entity.Components.Add(component.Build(entity));
            });
            return entity;
        }
    }
}