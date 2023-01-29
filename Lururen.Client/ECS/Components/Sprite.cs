using Lururen.Client.ECS.Components;
using Lururen.Client.Graphics;
using Lururen.Client.Graphics.Drawables;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Drawing2D
{
    public class Sprite : Component
    {
        public Texture2D Texture { get; }

        public Sprite(Texture2D texture)
        {
            this.Texture = texture;
            Context2D.Register(this);
        }

        public override void Init()
        {
            Texture.Init();
            var transform = Entity.GetComponent<Transform2D>();
        }

        public override void Update(double deltaTime)
        {
            Texture.Draw(deltaTime);
        }
    }
}
