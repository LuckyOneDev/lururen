using Lururen.Client.ECS.Planar.Components;
using Lururen.Client.Graphics;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.Graphics.Shapes;
using Lururen.Common.Extensions;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using StbImageSharp;
using System.Diagnostics;

namespace Lururen.Client.ECS.Planar.Systems
{
    public class Renderer2D : ISystem<SpriteRenderer>
    {
        #region Singleton
        private static Renderer2D instance;

        private Renderer2D() { }

        public static Renderer2D GetInstance()
        {
            instance ??= new Renderer2D();
            return instance;
        }
        #endregion

        private readonly Dictionary<FileAccessor, List<SpriteRenderer>> Components = new();
        protected static Game Window { get; set; }

        public void Init(Game window)
        {
            GL.Enable(EnableCap.DepthTest);
            StbImage.stbi_set_flip_vertically_on_load(1);
            Window = window;
        }

        public void Register(SpriteRenderer component)
        {
            Components.AddOrCreateList(component.Texture.Accessor, component);
        }

        protected static bool IsVisible(SpriteRenderer spriteRenderer, Camera camera)
        {
            RectangleF spriteRect = new(
                spriteRenderer.Transform.Position.X,
                spriteRenderer.Transform.Position.Y,
                spriteRenderer.Texture.Width * spriteRenderer.Transform.Scale,
                spriteRenderer.Texture.Height * spriteRenderer.Transform.Scale
            );

            RectangleF viewRect = new(
                -camera.GetPositionCorrector().X,
                -camera.GetPositionCorrector().Y,
                camera.ViewportSize.X,
                camera.ViewportSize.Y
            );

            return viewRect.IntersectsWith(spriteRect);
        }

        protected static List<SpriteRenderer> FilterSprites(List<SpriteRenderer> sprites, Camera camera)
        {
            return sprites.AsParallel().Where(x => IsVisible(x, camera)).ToList();
        }

        /// <summary>
        /// Rendering pipeline
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(double deltaTime)
        {
            var camera = Camera.GetActiveCamera();

            if (camera != null)
            {
                SpriteRenderer.Shader.Use();
                GLRect.Use();

                foreach (var entry in Components)
                {
                    var accessor = entry.Key;
                    var texture = FileHandle<GLTexture>.GetInstance().Get(accessor);
                    texture.Use();
                    var visibleSprites = FilterSprites(entry.Value, camera);
                    visibleSprites.ForEach(sprite =>
                    {
                        sprite.Update(deltaTime);
                        sprite.Render(camera);
                    });
                }
            }
        }

        public void Unregister(SpriteRenderer component)
        {
            Components.RemoveFromList(component);
        }

        public static Vector2i WindowSize => Window.ClientSize;
    }
}