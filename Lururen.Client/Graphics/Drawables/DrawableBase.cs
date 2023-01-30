using Lururen.Client.ECS;
using Lururen.Client.Graphics.Shaders;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Graphics.Drawables
{
    public abstract class DrawableBase : IDisposable
    {
        protected static Shader? Shader { get; set; } = null;
        protected float[] Vertices { get; set; }
        protected BufferUsageHint BufferUsageHint { get; }
        
        /// <summary>
        /// Raw float data
        /// </summary>
        protected int VertexBufferObject { get; private set; } = -1;

        /// <summary>
        /// Stores all of the state needed to supply vertex data
        /// </summary>
        protected int VertexArrayObject { get; private set; } = -1;

        public DrawableBase(float[] vertices, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
        {
            this.Vertices = vertices;
            this.BufferUsageHint = bufferUsageHint;
        }

        /// <summary>
        /// Define an array of generic vertex attribute data and enable it
        /// </summary>
        /// <param name="location">Index of the generic vertex attribute to be modified</param>
        /// <param name="vertexSize">Number of components per generic vertex attribute. Must be 1, 2, 3, 4</param>
        /// <param name="stride">Offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array</param>
        /// <param name="pointer">Offset of the first component of the first generic vertex attribute in the array</param>
        protected void SetVertexAttribPointer(int location, int vertexSize, int stride, nint pointer = 0)
        {
            if (vertexSize < 1 || vertexSize > 4) throw new ArgumentException(nameof(vertexSize));
            GL.VertexAttribPointer(location, vertexSize, VertexAttribPointerType.Float, false, stride * sizeof(float), pointer * sizeof(float));
            GL.EnableVertexAttribArray(location);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bufferTarget"></param>
        /// <returns></returns>
        protected int InitBuffer<T>(T[] data, BufferTarget bufferTarget) where T : struct
        {
            int buffer = GL.GenBuffer();
            GL.BindBuffer(bufferTarget, buffer);
            GL.BufferData(bufferTarget, data.Length * sizeof(float), data, BufferUsageHint);
            return buffer;
        }

        private void InitBuffers()
        {
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            VertexBufferObject = InitBuffer(Vertices, BufferTarget.ArrayBuffer);
        }


        public virtual void Init()
        {
            InitBuffers();
        }
        public virtual void Draw(double deltaTime)
        {
            Shader.Use();
            GL.BindVertexArray(VertexArrayObject);
        }

        public virtual void Dispose()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
        }
    }
}
