using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.Graphics.OpenGL.Drawables.Shapes
{
    public class Rectangle : DrawableGeometry
    {
        public int ElementBufferObject { get; private set; }
        public uint[] Indices { get; private set; }

        public Rectangle(float[] vertices, Color4 color) : base(vertices, Shaders.Shader.FromResource("Lururen.Client.Graphics.OpenGL.Shader.MonoColor"))
        {
            if (vertices.Length != 12) throw new ArgumentException("Vertices should be exacly 12 floats long");
        }

        public override void Init()
        {
            base.Init();
            InitVAO(3);

            Indices = new uint[] {
                0, 1, 3, // First triangle
                1, 2, 3  // Second triangle
            };

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
        }

        public override void Draw()
        {
            base.Draw();
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}