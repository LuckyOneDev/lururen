using Lururen.Client.Graphics;
using Lururen.Client.Graphics.Drawables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.ECS.Drawing2D
{
    public class Sprite : IDrawable
    {
        public Entity Entity { get; set; }
        public Texture2D Texture { get; }

        public Sprite(Texture2D texture) 
        {
            this.Texture = texture;
            Context2D.Register(this);
        }

        public void Init()
        {
            Texture.Init();
        }

        public void Update(double deltaTime)
        {
            Texture.Update(deltaTime);
        }
    }
}
