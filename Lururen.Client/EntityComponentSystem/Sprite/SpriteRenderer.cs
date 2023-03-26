using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.Graphics.Texturing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.ComponentModel.DataAnnotations;

namespace Lururen.Client.EntityComponentSystem.Sprite
{
    /// <summary>
    /// Handles Texture2D rendering in 2D space.
    /// </summary>
    public sealed class SpriteComponent : Component
    {
        /// <summary>
        /// Shader used by this component.
        /// </summary>
        internal static GLShader GlShader = GLShader.FromResource("Lururen.Client.Graphics.Shaders.Texture2D");

        /// <summary>
        /// Sprite's texture.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Creates instance of SpriteRenderer.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="texture"></param>
        public SpriteComponent(Entity entity) : base(entity)
        {
            Register(this);
        }

        /// <summary>
        /// Normalized texture offset. E.g. [1,1] means that texture would appear one width to the right
        /// and one texture height to the left.
        /// [0,0] is default.
        /// </summary>
        public Vector2 Pivot { get; set; } = Vector2.Zero;

        /// <summary>
        /// Gets or sets Texture and updates spriteRect accordingly.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                GlRect = GLRect.FromSizes(value.Width * Transform.Scale, value.Height * Transform.Scale);
                texture = value;
            }
        }

        /// <summary>
        /// Rect for OpenGL drawing.
        /// </summary>
        internal GLRect GlRect { get; set; }

        public override void Dispose()
        {
            Unregister(this);
            base.Dispose();
            GlRect.Dispose();
        }

        public override void Update(double deltaTime)
        {
            // Size correction 
            GlRect.SetSizes(Texture.Width * Transform.Scale, Texture.Height * Transform.Scale);
        }
        internal int GetBufferOffset() => GlRect.GetBufferOffset();

        /// <summary>
        /// Computes shader unifroms and sets them.
        /// </summary>
        /// <param name="camera"></param>
        private void ComputeShaderValues(Camera camera)
        {
            var correctedPosition = Transform.Position + camera.GetPositionCorrector();
            var correctedRotation = Transform.Rotation + camera.GetRotationCorrector();

            Matrix4 model = Matrix4.CreateRotationZ(correctedRotation);
            Matrix4 view = Matrix4.CreateTranslation(correctedPosition);
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, camera.ViewportSize.X, 0, camera.ViewportSize.Y, 0, 100f);
            Matrix4 pivot = Matrix4.CreateTranslation(new Vector3(-Pivot.X * Texture.Width, -Pivot.Y * Texture.Height, 0));

            GlShader.SetMatrix4("model", model);
            GlShader.SetMatrix4("pivot", pivot);
            GlShader.SetMatrix4("view", view);
            GlShader.SetMatrix4("projection", projection);
            GlShader.SetFloat("layer", Transform.Position.Z);
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
                GetBufferOffset() * 4); // I have no clue why is this 4. OpenGL is hard.
        }

        public override void Init()
        {
        }
    }
}