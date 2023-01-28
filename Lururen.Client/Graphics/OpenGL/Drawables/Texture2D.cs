using Lururen.Client.Graphics.OpenGL.Shaders;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Lururen.Client.Graphics.OpenGL.Drawables
{
    public class Texture2D : IDrawable
    {
        public Stream TextureStream { get; }
        public Shader Shader { get; }
        public Vector2 Postion { get; }
        public float Scale { get; }
        public float[] Vertices { get; private set; }
        public int VertexBufferObject { get; private set; }
        public int VertexArrayObject { get; private set; }

        public Texture2D(Stream textureStream, Vector2 position, float scale = 1.0f)
        {
            this.TextureStream = textureStream;
            this.Shader = Shader.FromResource("Lururen.Client.Graphics.OpenGL.Shaders.Texture2D");
            this.Postion = position;
            this.Scale = scale;
        }

        public void Init()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(TextureStream, ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                image.Data);

            Vertices = new float[]
            {
                Postion.X,  Postion.Y, 0.0f, 1.0f, 1.0f, // top right
                Postion.Y,  Postion.Y - Scale , 0.0f, 1.0f, 0.0f, // bottom right
                Postion.X - Scale,  Postion.Y - Scale, 0.0f, 0.0f, 0.0f, // bottom left
                Postion.X - Scale,  Postion.Y, 0.0f, 0.0f, 1.0f  // top left
            };

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            int texCoordLocation = Shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        public void Draw()
        {
            Shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 5);
        }
    }
}