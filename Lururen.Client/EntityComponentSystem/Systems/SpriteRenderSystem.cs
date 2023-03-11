using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.EntityComponentSystem.Components;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.Graphics;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.Graphics.Helpers;
using Lururen.Client.Graphics.Texturing;
using Lururen.Client.ResourceManagement;
using Lururen.Common.Extensions;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using StbImageSharp;

namespace Lururen.Client.EntityComponentSystem.Systems
{
    /// <summary>
    /// Implements rendering pipeline. 
    /// Calls SpriteRenderers with needed optmizations.
    /// </summary>
    public class SpriteRenderSystem : ISystem<SpriteComponent>
    {
        private Dictionary<FileAccessor, List<SpriteComponent>> Components = new();
        private List<SpriteComponent> NoTextureSprites { get; set; } = new();
        protected static GLWindow Window { get; set; }
        public CameraSystem CameraSystem { get; private set; }
        public Application Application { get; private set; }
        public void Init(GLWindow window, CameraSystem camSys, Application app)
        {
            // Enable depth test.
            GL.Enable(EnableCap.DepthTest);
            // Initialize imaging library. So images are compactible with OpenGL
            StbImage.stbi_set_flip_vertically_on_load(1);
            Window = window;

            this.CameraSystem = camSys;
            this.Application = app;
        }

        public void Register(SpriteComponent component)
        {
            if (component.Texture is null)
            {
                NoTextureSprites.Add(component);
                return;
            }
            Components.AddOrCreateList(component.Texture.Accessor, component);
        }

        protected void CheckNoTextureSprites()
        {
            for (int i = 0; i < NoTextureSprites.Count; i++)
            {
                var component = NoTextureSprites[i];
                if (component.Texture is not null)
                {
                    Components.AddOrCreateList(component.Texture.Accessor, component);
                }
                NoTextureSprites.RemoveAt(i);
                i--;
            }
        }

        protected static bool IsVisible(SpriteComponent spriteRenderer, Camera camera)
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
        protected static List<SpriteComponent> FilterSprites(List<SpriteComponent> sprites, Camera camera)
        {
            return sprites.AsParallel().Where(x => IsVisible(x, camera) && x.IsActive()).ToList();
        }

        /// <summary>
        /// Computes shader unifroms and sets them.
        /// </summary>
        /// <param name="camera"></param>
        private void ComputeShaderValues(SpriteComponent sprite, Camera camera)
        {
            var correctedPosition = sprite.Transform.Position + camera.GetPositionCorrector() + new Vector3(-sprite.Pivot.X * sprite.Texture.Width, -sprite.Pivot.Y * sprite.Texture.Height, 0);
            var correctedRotation = sprite.Transform.Rotation + camera.GetRotationCorrector();

            Matrix4 model = Matrix4.CreateRotationZ(correctedRotation);
            Matrix4 view = Matrix4.CreateTranslation(correctedPosition.X, correctedPosition.Y, correctedPosition.Z);
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, camera.ViewportSize.X, 0, camera.ViewportSize.Y, 0, 100f);

            SpriteComponent.Shader.SetMatrix4("model", model);
            SpriteComponent.Shader.SetMatrix4("view", view);
            SpriteComponent.Shader.SetMatrix4("projection", projection);
            SpriteComponent.Shader.SetFloat("layer", sprite.Transform.Position.Z);
        }

        public void RenderSprite(SpriteComponent sprite, Camera camera)
        {
            ComputeShaderValues(sprite, camera);

            // Buffer is guaranteed to be filled with data already.
            GL.DrawElementsBaseVertex(
                PrimitiveType.Triangles,
                GLRect.indices.Length,
                DrawElementsType.UnsignedInt,
                0,
                sprite.GetBufferOffset() * 4); // I have no clue why is this 4. OpenGL is hard.
        }

        /// <summary>
        /// Rendering pipeline
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(double deltaTime)
        {
            var camera = CameraSystem.GetActiveCamera();
            CheckNoTextureSprites();

            if (camera != null)
            {
                SpriteComponent.Shader.Use();
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
                        RenderSprite(sprite, camera);
                    });
                }

                OpenGLHelper.CheckErrors();
            }
        }

        public void Unregister(SpriteComponent component)
        {
            Components.RemoveFromList(component);
        }

        /// <summary>
        /// Size of current game window.
        /// </summary>
        public static Vector2i WindowSize => Window.ClientSize;
    }
}