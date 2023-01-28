using OpenTK.Mathematics;

namespace Lururen.Client.Graphics.OpenGL.Drawables
{
    public class VerticeCollection
    {
        public VerticeCollection()
        { }

        public VerticeCollection(IEnumerable<float> vertices)
        {
            this.vertices = vertices.ToList();
        }

        private List<float> vertices = new List<float>();
        public float[] Scalar => vertices.ToArray();

        public void Add(float x = 0, float y = 0, float z = 0)
        {
            vertices.Add(x);
            vertices.Add(y);
            vertices.Add(z);
        }

        public static VerticeCollection TextureRect(Vector2 bottomLeftCorner, Vector2 topRightCorner)
        {
            return new VerticeCollection
                (
                    new float[]
                    {
                        topRightCorner.X, topRightCorner.Y, 0.0f, 1.0f, 1.0f,    // top right
                        topRightCorner.X, bottomLeftCorner.Y, 0.0f, 1.0f, 0.0f,  // bottom right
                        bottomLeftCorner.X, bottomLeftCorner.Y, 0.0f, 0.0f, 0.0f,  // bottom left
                        bottomLeftCorner.X, topRightCorner.Y, 0.0f,  0.0f, 1.0f  // top left
                    }
                );
        }
    }
}