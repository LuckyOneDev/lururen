using Lururen.Client.Graphics.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Lururen.Client.Graphics.Drawables
{
    public class Texture2D : DrawableBase
    {
        protected static uint[] Indices { get; } = new uint[] {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
        };

        protected ResourceHandle Texture { get; }
        protected int ElementBufferObject { get; set; } = -1;
        protected int TextureHandle { get; set; } = -1;

        public Texture2D(ResourceHandle texture,
                         Vector2 topRightCorner,
                         Vector2 bottomLeftCorner,
                         BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw) :
            base(new float[]
            {
                 // positions[0..2]                                 // texture coords[3..4]
                 topRightCorner.X,   topRightCorner.Y,   0.0f,      1.0f, 1.0f,  // top right
                 topRightCorner.X,   bottomLeftCorner.Y, 0.0f,      1.0f, 0.0f,  // bottom right
                 bottomLeftCorner.X, bottomLeftCorner.Y, 0.0f,      0.0f, 0.0f,  // bottom left
                 bottomLeftCorner.X, topRightCorner.Y,   0.0f,      0.0f, 1.0f   // top left
            }, bufferUsageHint)
        {
            this.Texture = texture;

            if (Shader == null)
            {
                Shader = Shader.FromResource("Lururen.Client.Graphics.Shaders.Texture2D");
            }
        }

        protected int LoadTexture(ResourceHandle texture)
        {
            int TextureHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromMemory(texture.GetBytes(), ColorComponents.RedGreenBlueAlpha);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.TexImage2D(TextureTarget.Texture2D,
                          0,
                          PixelInternalFormat.Rgba,
                          image.Width,
                          image.Height,
                          0,
                          PixelFormat.Rgba,
                          PixelType.UnsignedByte,
                          image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return TextureHandle;
        }

        // ???
        protected int CastTextureUnit(TextureUnit unit) => unit switch
        {
            TextureUnit.Texture0 => 0,
            TextureUnit.Texture1 => 1,
            TextureUnit.Texture2 => 2,
            TextureUnit.Texture3 => 3,
            TextureUnit.Texture4 => 4,
            TextureUnit.Texture5 => 5,
            TextureUnit.Texture6 => 6,
            TextureUnit.Texture7 => 7,
            TextureUnit.Texture8 => 8,
            TextureUnit.Texture9 => 9,
            TextureUnit.Texture10 => 10,
            TextureUnit.Texture11 => 11,
            TextureUnit.Texture12 => 12,
            TextureUnit.Texture13 => 13,
            TextureUnit.Texture14 => 14,
            TextureUnit.Texture15 => 15,
            TextureUnit.Texture16 => 16
        };

        protected int UseTexture(ResourceHandle texture, string uniformName, TextureUnit textureUnit)
        {
            int TextureHandle = LoadTexture(texture);
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            Shader.SetInt(uniformName, CastTextureUnit(textureUnit));
            return TextureHandle;
        }

        public override void Init()
        {
            base.Init();
            ElementBufferObject = InitBuffer(Indices, BufferTarget.ElementArrayBuffer);

            Shader.Use();
            TextureHandle = UseTexture(Texture, "texture0", TextureUnit.Texture16);

            SetVertexAttribPointer(0, 3, 5);    // vec3 aPosition
            SetVertexAttribPointer(1, 2, 5, 3); // vec2 aTexCoord
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public override void Dispose()
        {
            base.Dispose();
            GL.DeleteBuffer(ElementBufferObject);
        }
    }
}