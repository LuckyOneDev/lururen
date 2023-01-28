using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Graphics.OpenGL.Drawables.Shapes
{
    public class Triangle : DrawableGeometry
    {
        public Triangle(float[] vertices) : base(vertices, Shaders.Shader.FromResource("Lururen.Client.Graphics.OpenGL.Shader.MonoColor"))  
        {
            if (vertices.Length != 9) throw new ArgumentException("Vertices should be exacly 9 floats long");
        }

        public override void Init()
        {
            base.Init();
            InitVAO(3);
        }

        public override void Draw()
        {
            base.Draw();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }
    }
}
