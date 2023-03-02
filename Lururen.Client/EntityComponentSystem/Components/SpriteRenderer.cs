using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.EntityComponentSystem.Generic;
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
            Register(this);
        }

        public override void Update(double deltaTime)
        {
            // Size correction 
            Rect.SetSizes(Texture.Width * Transform.Scale, Texture.Height * Transform.Scale);
        }

        public override void Dispose()
        {
            Unregister(this);
            base.Dispose();
            Rect.Dispose();
        }

        internal int GetBufferOffset() => Rect.GetBufferOffset();
    }
}