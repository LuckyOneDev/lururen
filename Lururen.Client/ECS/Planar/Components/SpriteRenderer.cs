using Lururen.Client.ECS.Planar.Systems;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.Graphics.Shapes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Components
{
    public class SpriteRenderer : Component
    {
        public Transform2D? Transform { get; private set; }
        public Texture2D Texture { get; set; }

        internal static GLShader Shader = GLShader.FromResource("Lururen.Client.Graphics.Shaders.Texture2D");
        
        protected GLRect Rect { get; set; }

        public SpriteRenderer(Texture2D texture)
        {
            Texture = texture;
            Renderer2D.GetInstance().Register(this);
        }

        public override void Init()
        {
            //Texture.Init();
            Transform = Entity.GetComponent<Transform2D>();
            Rect = GLRect.FromSizes(Texture.Width * Transform.Scale, Texture.Height * Transform.Scale);
        }

        private void ComputeShaderValues(Camera camera) 
        {
            var correctedPosition = Transform.Position - camera.Transform.Position;
            var correctedRotation = Transform.Rotation - camera.Transform.Rotation;

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
            GLRect.Use();
            GL.DrawElements(PrimitiveType.Triangles, GLRect.indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public override void Update(double deltaTime)
        {
            if (Transform != null)
            {
                // Size correction 
                GLRect.SetSizes(Texture.Width * Transform.Scale, Texture.Height * Transform.Scale);
            }
        }
    }
}