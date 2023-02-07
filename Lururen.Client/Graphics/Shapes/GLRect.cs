using Newtonsoft.Json.Linq;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.Graphics.Shapes
{
    public class GLRect : IDisposable
    {
        private static PrecalculationCollection<float> Instances { get; set; } = new(16);

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

        public Vector2 TopRightCorner { get; set; }
        public Vector2 BottomLeftCorner { get; set; }

        public GLRect(Vector2 topRightCorner, Vector2 bottomLeftCorner)
        {
            TopRightCorner = topRightCorner;
            BottomLeftCorner = bottomLeftCorner;
            Index = Instances.Add(BuildVertexArray(TopRightCorner, BottomLeftCorner));
        }

        public void SetSizes(float width, float height)
        {
            TopRightCorner = new Vector2(width, height);
            BottomLeftCorner = Vector2.Zero;
            Instances.Set(Index, BuildVertexArray(TopRightCorner, BottomLeftCorner));
        }

        public static uint[] GenIndices(uint offset = 0)
        {
            offset = offset * (uint)indices.Length;
            return indices.Select(x => x + offset).ToArray();
        }

        public static readonly uint[] indices = new uint[] {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
        };

        static GLRect()
        {
            VBO = OpenGLHelper.InitBuffer(BufferTarget.ArrayBuffer);
            VAO = OpenGLHelper.InitVertexArrayObject();
            EBO = OpenGLHelper.InitBuffer(indices, BufferTarget.ElementArrayBuffer);
        }

        #region OpenGL handles

        protected static int EBO { get; }

        protected static int VAO { get; }

        protected static int VBO { get; set; }
        public static float[] VertexArray { get; private set; }
        public uint Index { get; }

        #endregion OpenGL handles

        public static void Prepare()
        {
            GL.BindVertexArray(VAO);
            OpenGLHelper.SetVertexAttribPointer(0, 2, 4);    // vec2 aPosition
            OpenGLHelper.SetVertexAttribPointer(1, 2, 4, 2); // vec2 aTexCoord
            OpenGLHelper.SetBuffer(Instances.GetJoined(), BufferTarget.ArrayBuffer);
        }

        public void Use()
        {
            OpenGLHelper.SetBuffer(GenIndices(Index), BufferTarget.ElementArrayBuffer);
        }

        public void Dispose()
        {
            Instances.Remove(Index);
        }

        internal static void Reserve(int v)
        {
            Instances.Reserve(v);
        }
    }
}