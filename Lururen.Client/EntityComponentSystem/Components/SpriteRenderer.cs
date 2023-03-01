using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Systems;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.Graphics.Texturing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.ComponentModel.DataAnnotations;

namespace Lururen.Client.EntityComponentSystem.Components
{
    /// <summary>
    /// Handles Texture2D rendering in 2D space.
    /// </summary>
    public class SpriteComponent : Component
    {
        private Texture2D texture;
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                Rect = GLRect.FromSizes(value.Width * Transform.Scale, value.Height * Transform.Scale);
                texture = value;
            }
        }

        /// <summary>
        /// Normalized texture offset. E.g. [1,1] means that texture would appear one width to the right
        /// and one texture height to the left.
        /// [0,0] is default.
        /// </summary>
        public Vector2 Pivot { get; set; } = Vector2.Zero;

        internal static GLShader Shader = GLShader.FromResource("Lururen.Client.Graphics.Shaders.Texture2D");

        protected GLRect Rect { get; set; }

        /// <summary>
        /// Creates instance of SpriteRenderer.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="texture"></param>
        public SpriteComponent(Entity entity) : base(entity)
        {
            Entity.World.Application.SystemManager.RegisterComponent(this); // Wtf this lift
        }

        /// <summary>
        /// Computes shader unifroms and sets them.
        /// </summary>
        /// <param name="camera"></param>
        private void ComputeShaderValues(Camera camera)
        {
            var correctedPosition = Transform.Position + camera.GetPositionCorrector() + new Vector3(-Pivot.X * Texture.Width, -Pivot.Y * Texture.Height, 0);
            var correctedRotation = Transform.Rotation + camera.GetRotationCorrector();

            Matrix4 model = Matrix4.CreateRotationZ(correctedRotation);
            Matrix4 view = Matrix4.CreateTranslation(correctedPosition.X, correctedPosition.Y, 0.0f);
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, camera.ViewportSize.X, 0, camera.ViewportSize.Y, 0, 100f);

            Shader.SetMatrix4("model", model);
            Shader.SetMatrix4("view", view);
            Shader.SetMatrix4("projection", projection);
        }

        public void Render(Camera camera)
        {
            ComputeShaderValues(camera);

            // Buffer is guaranteed to be filled with data already.
            GL.DrawElementsBaseVertex(
                PrimitiveType.Triangles,
                GLRect.indices.Length,
                DrawElementsType.UnsignedInt,
                0,
                Rect.GetBufferOffset() * 4); // I have no clue why is this 4. OpenGL is hard.
        }

        public override void Update(double deltaTime)
        {
            // Size correction 
            Rect.SetSizes(Texture.Width * Transform.Scale, Texture.Height * Transform.Scale);
        }

        public override void Dispose()
        {
            //Renderer2D.GetInstance().Unregister(this);
            base.Dispose();
            Rect.Dispose();
        }
    }
}