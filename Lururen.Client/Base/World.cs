using Lururen.Client.EntityComponentSystem.Base;

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

        public Entity CreateEntity(Prefab prefab, float x, float y, float z)
        {
            var entity = new Entity(this);
            entity.Transform.Position = new OpenTK.Mathematics.Vector3(x, y, z);
            prefab.Configurator(entity);
            return entity;
        }
    }
}