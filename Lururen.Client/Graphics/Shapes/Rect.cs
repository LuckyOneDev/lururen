using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.Graphics.Shapes
{
    public class Rect
    {
        public static Rect FromSize(float size)
        {
            return new Rect(new Vector2(size, size), Vector2.Zero);
        }
        public Rect(Vector2 topRightCorner, Vector2 bottomLeftCorner)
        {
            Vertices = new float[]
            {
                 // positions[0..2]                                 // texture coords[3..4]
                 topRightCorner.X,   topRightCorner.Y,   0.0f,      1.0f, 1.0f,  // top right
                 topRightCorner.X,   bottomLeftCorner.Y, 0.0f,      1.0f, 0.0f,  // bottom right
                 bottomLeftCorner.X, bottomLeftCorner.Y, 0.0f,      0.0f, 0.0f,  // bottom left
                 bottomLeftCorner.X, topRightCorner.Y,   0.0f,      0.0f, 1.0f   // top left
            };

            VAO = OpenGLHelper.InitVertexArrayObject();
            VBO = OpenGLHelper.InitBuffer(Vertices, BufferTarget.ArrayBuffer);
            EBO = OpenGLHelper.InitBuffer(indices, BufferTarget.ElementArrayBuffer);

            OpenGLHelper.SetVertexAttribPointer(0, 3, 5);    // vec3 aPosition
            OpenGLHelper.SetVertexAttribPointer(1, 2, 5, 3); // vec2 aTexCoord
        }

        public static uint[] indices { get; } = new uint[] {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
        };

        protected float[] Vertices { get; set; }


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

            OpenGLHelper.SetBuffer(Vertices, BufferTarget.ArrayBuffer);
        }

        #region OpenGL handles

        protected int EBO { get; set; }
        protected int VAO { get; set; }
        protected int VBO { get; set; }
        #endregion OpenGL handles
        public void Use()
        {
            GL.BindVertexArray(VAO);
        }
    }
}