﻿using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Lururen.Client.Graphics.Generic
{
    public class GLTexture : IByteConstructable<GLTexture>
    {
        public GLTexture(byte[] texture)
        {
            TextureParameters = new TextureParameters();
            ImageResult Image = ImageResult.FromMemory(texture, ColorComponents.RedGreenBlueAlpha);
            Width = Image.Width;
            Height = Image.Height;
            Handle = OpenGLHelper.LoadTexture(Image, TextureParameters);
        }

        public GLTexture(byte[] texture, TextureParameters textureParameters)
        {
            TextureParameters = textureParameters;
            ImageResult Image = ImageResult.FromMemory(texture, ColorComponents.RedGreenBlueAlpha);
            Width = Image.Width;
            Height = Image.Height;
            Handle = OpenGLHelper.LoadTexture(Image, TextureParameters);
        }

        public int Handle { get; set; }
        public int Width { get; }
        public int Height { get; }
        public TextureParameters TextureParameters { get; }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public static GLTexture FromBytes(byte[] bytes)
        {
            return new GLTexture(bytes);
        }
    }
}