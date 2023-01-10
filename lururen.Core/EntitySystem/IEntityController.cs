using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lururen.EntitySystem
{
    public interface IEntityController
    {
        public Entity Parent { get; set; }
        public abstract void Init();
        public abstract void Update();
        public abstract void Dispose();
    }
}
