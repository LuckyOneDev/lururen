using Lururen.Client.ECS.Planar.Components;
using Lururen.Client.Graphics;
using Lururen.Client.Graphics.Generic;
using Lururen.Common.Extensions;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Lururen.Client.ECS.Planar.Systems
{
    public class Renderer2D : BaseSystem<SpriteRenderer>
    {
        #region Singleton
        private static Renderer2D instance;

        private Renderer2D() { }

        public static Renderer2D GetInstance()
        {
            if (instance == null)
                instance = new Renderer2D();
            return instance;
        }
        #endregion

        private Dictionary<FileAccessor, List<SpriteRenderer>> Components = new();
        protected static Game Window { get; set; }

        public void Init(Game window)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            Window = window;
        }

        public void Register(SpriteRenderer component)
        {
            Components.AddOrCreateList(component.Texture.Accessor, component);
        }

        public void Update(double deltaTime)
        {
            var camera = Camera.GetActiveCamera();
            SpriteRenderer.Shader.Use();

            if (camera != null)
            {
                foreach (var entry in Components) 
                {
                    var accessor = entry.Value.First().Texture.Accessor;
                    var texture = FileHandle<GLTexture>.GetInstance().Get(accessor);
                    texture.Use();

                    entry.Value.ForEach(sprite =>
                    {
                        sprite.Update(deltaTime);
                        sprite.Render(camera);
                    });
                }
            }
        }

        public static Vector2i WindowSize => Window.ClientSize;
    }
}