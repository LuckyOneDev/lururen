using Lururen.Client.ECS;
using Lururen.Client.Graphics.Shapes;
using Lururen.Common.EntitySystem;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client
{
    public class EntityManager
    {
        protected List<Entity> Entities { get; set; } = new();
        public EntityManager() { }

        public T GetEntityByType<T>() where T : Entity => (T)Entities.Find(x => x.GetType() == typeof(T));

        public void AddEntity(Entity ent)
        {
            Entities.Add(ent);
        }

        public void RemoveEntity(Entity ent)
        {
            Entities.Remove(ent);
        }
    }
}
