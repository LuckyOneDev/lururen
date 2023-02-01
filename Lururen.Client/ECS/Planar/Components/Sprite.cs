using Lururen.Client.ECS.Planar.Systems;
using Lururen.Client.Graphics.Generic;
using Lururen.Client.Graphics.Shapes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Components
{
    public class Sprite : Component
    {
        protected static Shader Shader = Shader.FromResource("Lururen.Client.Graphics.Shaders.Texture2D");
        protected Texture Texture { get; set; }
        protected Rect Rect { get; set; }

        public Transform2D? Transform { get; private set; }

        public Sprite(Texture texture)
        {
            Texture = texture;
            SpriteRenderer.Register(this);
        }

        public override void Init()
        {
            Texture.Init();
            Transform = Entity.GetComponent<Transform2D>();
            Rect = Rect.FromSize(Transform.Scale);
        }

        public override void Update(double deltaTime)
        {
            if (Transform != null)
            {
                // Size correction
                var size = Transform.Scale;
                Rect.SetSize(size);

                var camera = SpriteRenderer.GetActiveCamera();
                if (camera is not null)
                {
                    // Position correction
                    var correctedPosition = Transform.Position - camera.Transform.Position;
                    var correctedRotation = Transform.Rotation - camera.Transform.Rotation;

                    Matrix4 model = Matrix4.CreateRotationZ(correctedRotation);
                    Matrix4 view = Matrix4.CreateTranslation(correctedPosition.X, correctedPosition.Y, 0.0f);
                    Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, camera.ViewportSize.X, 0, camera.ViewportSize.Y, 0, 100f);

                    Shader.SetMatrix4("model", model);
                    Shader.SetMatrix4("view", view);
                    Shader.SetMatrix4("projection", projection);

                    Rect.Use();
                    Texture.Use();
                    Shader.Use();

                    GL.DrawElements(PrimitiveType.Triangles, Rect.indices.Length, DrawElementsType.UnsignedInt, 0);
                    GL.Viewport(0, 0, camera.ViewportSize.X, camera.ViewportSize.Y);
                }
            }
        }
    }
}