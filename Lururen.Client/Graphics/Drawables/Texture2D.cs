using Lururen.Client.Graphics.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;

namespace Lururen.Client.Graphics.Drawables
{
    public class Texture2D
    {
        public Texture2D(ResourceHandle resourceHandle, Vector2 topRightCorner, Vector2 bottomLeftCorner)
        {
            this.ResourceHandle = resourceHandle;
            this.TextureParameters = new TextureParameters();
            SetVertices(topRightCorner, bottomLeftCorner);
        }

        public Texture2D(ResourceHandle resourceHandle, Vector2 topRightCorner, Vector2 bottomLeftCorner, TextureParameters textureParameters)
        {
            this.ResourceHandle = resourceHandle;
            this.TextureParameters = textureParameters;
            SetVertices(topRightCorner, bottomLeftCorner);
        }

        public void SetVertices(Vector2 topRightCorner, Vector2 bottomLeftCorner)
        {
            Vertices = new float[]
            {
                 // positions[0..2]                                 // texture coords[3..4]
                 topRightCorner.X,   topRightCorner.Y,   0.0f,      1.0f, 1.0f,  // top right
                 topRightCorner.X,   bottomLeftCorner.Y, 0.0f,      1.0f, 0.0f,  // bottom right
                 bottomLeftCorner.X, bottomLeftCorner.Y, 0.0f,      0.0f, 0.0f,  // bottom left
                 bottomLeftCorner.X, topRightCorner.Y,   0.0f,      0.0f, 1.0f   // top left
            };
        }

        protected static uint[] indices { get; } = new uint[] {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
        };

        protected static Shader Shader { get; } = Shader.FromResource("Lururen.Client.Graphics.Shaders.Texture2D");

        #region OpenGL handles
        protected float[] Vertices { get; set; }
        protected int VAO { get; set; }
        protected int VBO { get; set; }
        protected int EBO { get; set; }
        protected int TextureHandle { get; set; }
        #endregion

        public ResourceHandle ResourceHandle { get; }
        public TextureParameters TextureParameters { get; }
        private Image Image { get; set; }
        

        public void Init()
        {
            VAO = OpenGLHelper.InitVertexArrayObject();
            VBO = OpenGLHelper.InitBuffer(Vertices, BufferTarget.ArrayBuffer);
            EBO = OpenGLHelper.InitBuffer(indices, BufferTarget.ElementArrayBuffer);

            OpenGLHelper.SetVertexAttribPointer(0, 3, 5);    // vec3 aPosition
            OpenGLHelper.SetVertexAttribPointer(1, 2, 5, 3); // vec2 aTexCoord

            TextureHandle = OpenGLHelper.LoadTexture(ResourceHandle, TextureParameters);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            Shader.SetInt("texture0", 0);
        }

        public virtual void Use()
        {
            Shader.Use();
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}