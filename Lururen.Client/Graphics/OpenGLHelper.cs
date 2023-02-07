using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Graphics
{
    public static class OpenGLHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bufferTarget"></param>
        /// <returns></returns>
        public static int InitBuffer<T>(T[] data,
                                        BufferTarget bufferTarget,
                                        BufferUsageHint bufferUsageHint = BufferUsageHint.DynamicDraw) where T : struct
        {
            int buffer = GL.GenBuffer();
            GL.BindBuffer(bufferTarget, buffer);
            SetBuffer(data, bufferTarget, bufferUsageHint);
            return buffer;
        }

        public static int InitBuffer(
                                BufferTarget bufferTarget,
                                BufferUsageHint bufferUsageHint = BufferUsageHint.DynamicDraw)
        {
            int buffer = GL.GenBuffer();
            GL.BindBuffer(bufferTarget, buffer);
            return buffer;
        }

        public static void SetBuffer<T>(T[] data,
                                         BufferTarget bufferTarget,
                                         BufferUsageHint bufferUsageHint = BufferUsageHint.DynamicDraw) where T : struct
        {
            GL.BufferData(bufferTarget, data.Length * sizeof(float), data, bufferUsageHint);
        }

        public static int InitVertexArrayObject(Action<int>? callback = default)
        {
            int VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            if (callback != null) callback(VertexArrayObject);
            return VertexArrayObject;
        }

        public static int LoadTexture(ImageResult image, TextureParameters textureParameters)
        {
            int textureHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            SetTexParameters(textureParameters);
            
            GL.TexImage2D(TextureTarget.Texture2D,
                          0,
                          PixelInternalFormat.Rgba,
                          image.Width,
                          image.Height,
                          0,
                          PixelFormat.Rgba,
                          PixelType.UnsignedByte,
                          image.Data);
            
            if (textureParameters.GenerateMipMaps) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return textureHandle;
        }

        private static void SetTexParameters(TextureParameters textureParameters)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureParameters.TextureWrapS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureParameters.TextureWrapT);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureParameters.TextureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureParameters.TextureMagFilter);
        }

        /// <summary>
        /// Define an array of generic vertex attribute data and enable it
        /// </summary>
        /// <param name="location">Index of the generic vertex attribute to be modified</param>
        /// <param name="vertexSize">Number of components per generic vertex attribute. Must be 1, 2, 3, 4</param>
        /// <param name="stride">Offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array</param>
        /// <param name="pointer">Offset of the first component of the first generic vertex attribute in the array</param>
        public static void SetVertexAttribPointer(int location, int vertexSize, int stride, nint pointer = 0)
        {
            if (vertexSize < 1 || vertexSize > 4) throw new ArgumentException("Vertex size should be between 1 and 4");
            GL.VertexAttribPointer(location, vertexSize, VertexAttribPointerType.Float, false, stride * sizeof(float), pointer * sizeof(float));
            GL.EnableVertexAttribArray(location);
        }
    }
}
