using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using System.Reflection.Metadata;

namespace Lururen.Client.Graphics.Generic
{
    public class Texture
    {
        public Texture(ResourceHandle resourceHandle, Vector2 topRightCorner, Vector2 bottomLeftCorner)
        {
            ResourceHandle = resourceHandle;
            TextureParameters = new TextureParameters();
        }

        public Texture(ResourceHandle resourceHandle, TextureParameters textureParameters)
        {
            ResourceHandle = resourceHandle;
            TextureParameters = textureParameters;
        }

        #region OpenGL handles
        protected int TextureHandle { get; set; }
        #endregion

        public ResourceHandle ResourceHandle { get; }
        public TextureParameters TextureParameters { get; }

        public void Init()
        {
            TextureHandle = OpenGLHelper.LoadTexture(ResourceHandle, TextureParameters);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
        }
    }
}