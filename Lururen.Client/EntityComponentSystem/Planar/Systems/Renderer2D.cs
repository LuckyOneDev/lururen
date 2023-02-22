using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Components;
using Lururen.Client.Graphics;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.ResourceManagement;
using Lururen.Common.Extensions;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using StbImageSharp;

namespace Lururen.Client.EntityComponentSystem.Planar.Systems
{
    /// <summary>
    /// Implements rendering pipeline. 
    /// Calls SpriteRenderers with needed optmizations.
    /// </summary>
    public class Renderer2D : IRenderSystem<SpriteRenderer>
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
        protected static GLWindow Window { get; set; }

        public void Init(GLWindow window)
        {
            // Enable depth test.
            GL.Enable(EnableCap.DepthTest); 
            // Initialize imaging library. So images are compactible with OpenGL
            StbImage.stbi_set_flip_vertically_on_load(1);
            Window = window;
        }

        public void Register(SpriteRenderer component)
        {
            Components.AddOrCreateList(component.Texture.Accessor, component);
        }

        protected static bool IsVisible(SpriteRenderer spriteRenderer, Camera2D camera)
        {
            var diagonalA = (float)Math.Sqrt(Math.Pow(spriteRenderer.Texture.Width, 2) + Math.Pow(spriteRenderer.Texture.Height, 2));
            var diagonalB = (float)Math.Sqrt(Math.Pow(camera.ViewportSize.X, 2) + Math.Pow(camera.ViewportSize.Y, 2));

            RectangleF spriteRect = new(
                spriteRenderer.Transform.Position.X - diagonalA,
                spriteRenderer.Transform.Position.Y - diagonalA,
                2 * diagonalA * spriteRenderer.Transform.Scale,
                2 * diagonalA * spriteRenderer.Transform.Scale
            );

            RectangleF viewRect = new(
                -camera.GetPositionCorrector().X - diagonalB,
                -camera.GetPositionCorrector().Y - diagonalB,
                2 * diagonalB,
                2 * diagonalB
            );

            return viewRect.IntersectsWith(spriteRect);
        }

        /// <summary>
        /// Determines whitch part of sprite collecton is visible and
        /// therefore should be rendered.
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        protected static List<SpriteRenderer> FilterSprites(List<SpriteRenderer> sprites, Camera2D camera)
        {
            return sprites.AsParallel().Where(x => IsVisible(x, camera) && x.Active).ToList();
        }

        /// <summary>
        /// Rendering pipeline
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(double deltaTime)
        {
            var camera = Camera2D.GetActiveCamera();

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

        /// <summary>
        /// Size of current game window.
        /// </summary>
        public static Vector2i WindowSize => Window.ClientSize;
    }
}