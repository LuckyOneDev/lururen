using Lururen.Client.ECS.Planar.Systems;
using Lururen.Client.EntityComponentSystem.Planar;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.Graphics.Shapes;
using Lururen.Client.Graphics.Texturing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Components
{
    public class SpriteRenderer : Component2D, IRenderer
    {
        public Texture2D Texture { get; set; }
        public Vector2 Pivot { get; set; } = Vector2.Zero;

        internal static GLShader Shader = GLShader.FromResource("Lururen.Client.Graphics.Shaders.Texture2D");
        
        protected GLRect Rect { get; set; }

        public SpriteRenderer(Texture2D texture)
        {
            Texture = texture;
            Renderer2D.GetInstance().Register(this);
        }

        public override void Init(Entity ent)
        {
            base.Init(ent);
            Rect = GLRect.FromSizes(Texture.Width * Transform.Scale, Texture.Height * Transform.Scale);
        }

        private void ComputeShaderValues(Camera camera) 
        {
            var correctedPosition = Transform.Position + camera.GetPositionCorrector() + new Vector2(-Pivot.X * Texture.Width, -Pivot.Y * Texture.Height);
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
            GL.DrawElementsBaseVertex(
                PrimitiveType.Triangles,
                GLRect.indices.Length,
                DrawElementsType.UnsignedInt,
                0,
                Rect.Index.Value * 4); // I have no clue why is this 4. OpenGL is hard.
        }

        public override void Update(double deltaTime)
        {
            if (Transform != null)
            {
                // Size correction 
                Rect.SetSizes(Texture.Width * Transform.Scale, Texture.Height * Transform.Scale);
            }
        }

        public override void Dispose()
        {
            Renderer2D.GetInstance().Unregister(this);
            base.Dispose();
            Rect.Dispose();
        }
    }
}