using Lururen.Client.Graphics.OpenGL.Shaders;
using Lururen.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Graphics.OpenGL.Drawables
{
    public abstract class DrawableGeometry : IDrawable
    {
        public float[] Vertices { get; }
        protected int VertexBufferObject;
        protected int VertexArrayObject;

        protected Shader Shader;

        public DrawableGeometry(VerticeCollection vertices, Shader shader)
        {
            if (vertices == null) throw new ArgumentNullException("Vertices could not be null");
            Vertices = vertices.Scalar;
            Shader = shader;
        }

        public DrawableGeometry(float[] vertices, Shader shader)
        {
            if (vertices == null) throw new ArgumentNullException("Vertices could not be null");
            Vertices = vertices;
            Shader = shader;
        }

        protected void InitVAO(int size)
        {
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, size, VertexAttribPointerType.Float, false, size * sizeof(float), 0);
        }

        public virtual void Init()
        {
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
        }

        public virtual void Draw()
        {
            Shader.Use();
            GL.BindVertexArray(VertexArrayObject);
        }
    }
}
