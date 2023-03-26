using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.User;
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

        public override void Init()
        {
        }

        public override void Update(double deltaTime)
        {
            const float moveSpeed = 10f;
            const float rotSpeed = 1f;

            if (Input.IsKeyDown(Keys.A))
            {
                this.Transform.Rotation -= rotSpeed * (float)deltaTime;
            }

            if (Input.IsKeyDown(Keys.D))
            {
                this.Transform.Rotation += rotSpeed * (float)deltaTime;
            }

            if (Input.IsKeyDown(Keys.W))
            {
                this.Transform.Position.Y += moveSpeed * (float)deltaTime;
            }
        }
    }
}
