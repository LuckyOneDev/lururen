using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.Graphics.Shapes
{
    public class GLRect : IDisposable
    {
        public static GLRect FromSizes(float width, float height)
        {
            return new GLRect(new Vector2(width, height), Vector2.Zero);
        }

        protected static float[] BuildVertexArray(Vector2 topRightCorner, Vector2 bottomLeftCorner)
        {
            return new float[]
            {
                 // positions[0..2]                            // texture coords[3..4]
                 topRightCorner.X,   topRightCorner.Y,         1.0f, 1.0f,  // top right
                 topRightCorner.X,   bottomLeftCorner.Y,       1.0f, 0.0f,  // bottom right
                 bottomLeftCorner.X, bottomLeftCorner.Y,       0.0f, 0.0f,  // bottom left
                 bottomLeftCorner.X, topRightCorner.Y,         0.0f, 1.0f   // top left
            };
        }

        public GLRect(Vector2 topRightCorner, Vector2 bottomLeftCorner)
        {
            VBO = OpenGLHelper.InitBuffer(BuildVertexArray(topRightCorner, bottomLeftCorner), BufferTarget.ArrayBuffer);
        }

        public static readonly uint[] indices = new uint[] {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
        };

        public static void SetSizes(float width, float height)
        {
            SetVertices(new Vector2(width, height), Vector2.Zero);
        }

        public static void SetVertices(Vector2 topRightCorner, Vector2 bottomLeftCorner)
        {
            OpenGLHelper.SetBuffer(BuildVertexArray(topRightCorner, bottomLeftCorner), BufferTarget.ArrayBuffer);
        }

        #region OpenGL handles

        static GLRect()
        {
            VAO = OpenGLHelper.InitVertexArrayObject();
            EBO = OpenGLHelper.InitBuffer(indices, BufferTarget.ElementArrayBuffer);
        }

        protected static int EBO { get; }
        
        protected static int VAO { get; }

        protected int VBO { get; set; }

        #endregion OpenGL handles

        public static void Prepare()
        {
            OpenGLHelper.SetVertexAttribPointer(0, 2, 4);    // vec2 aPosition
            OpenGLHelper.SetVertexAttribPointer(1, 2, 4, 2); // vec2 aTexCoord
        }

        public static void Use()
        {
            GL.BindVertexArray(VAO);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(VBO);
        }
    }
}