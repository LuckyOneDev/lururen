using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.EntityComponentSystem.Components;
using Lururen.Client.EntityComponentSystem.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTestApp
{
    public class PlayerControl : UserComponent
    {
        public PlayerControl(Entity entity) : base(entity)
        {
        }

        public override void Init<T>(ISystem<T> system)
        {
            base.Init(system);
        }

        public override void Update(double deltaTime)
        {
            if (Input.IsKeyDown(Keys.A))
            {
                this.Transform.Position.X -= (float)deltaTime;
            }
        }
    }
}
