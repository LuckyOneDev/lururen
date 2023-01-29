using Lururen.Client.Graphics.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Graphics.Drawables
{
    /// <summary>
    /// Example of usage of drawable base
    /// </summary>
    public class Triangle : DrawableBase
    {
        public Color4 Color { get; set; }

        public Triangle(Vector2 ptA, Vector2 ptB, Vector2 ptC, Color4 color) : 
            base(new float[]
            {
                ptA.X, ptA.Y, 0,
                ptB.X, ptB.Y, 0,
                ptC.X, ptC.Y, 0
            })
        {
            this.Color = color;

            if (Shader == null)
            {
                Shader = Shader.FromResource("Lururen.Client.Graphics.Shaders.MonoColor");
            }
        }

        public override void Init()
        {
            base.Init();
            SetVertexAttribPointer(0, 3, 3);
            Shader.SetVector4("ourColor", ((Vector4)Color));
        }

        public override void Draw()
        {
            base.Draw();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }
    }
}
