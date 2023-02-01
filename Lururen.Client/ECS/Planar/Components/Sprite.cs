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
        protected Texture Texture { get; }
        protected Rect Rect { get; }

        public Transform2D? Transform { get; private set; }

        public Sprite(Texture texture)
        {
            Texture = texture;
            Rect = Rect.FromSize(100f);
            Context2D.Register(this);
        }

        public override void Init()
        {
            Texture.Init();
            Transform = Entity.GetComponent<Transform2D>();
        }

        Vector2 direction { get; set; } = new Vector2(1.0f, 1.0f);

        public override void Update(double deltaTime)
        {
            if (Transform != null)
            {
                var camera = Context2D.GetActiveCamera();
                if (camera is not null)
                {
                    if (Transform.X <= 0)
                    {
                        direction = new Vector2(1.0f, direction.Y);
                    }

                    if (Transform.X >= Context2D.WindowSize.X - 100f)
                    {
                        direction = new Vector2(-1.0f, direction.Y);
                    }

                    if (Transform.Y <= 0)
                    {
                        direction = new Vector2(direction.X, 1.0f);
                    }

                    if (Transform.Y >= Context2D.WindowSize.Y - 100f)
                    {
                        direction = new Vector2(direction.X, -1.0f);
                    }

                    Transform.X += 55 * direction.X * (float)deltaTime;
                    Transform.Y += 55 * direction.Y * (float)deltaTime;

                    Matrix4 model = Matrix4.CreateRotationZ(Transform.Rotation);
                    Matrix4 view = Matrix4.CreateTranslation(Transform.X, Transform.Y, 0.0f);
                    Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, Context2D.WindowSize.X, 0, Context2D.WindowSize.Y, 0, 100f);

                    Shader.SetMatrix4("model", model);
                    Shader.SetMatrix4("view", view);
                    Shader.SetMatrix4("projection", projection);

                    Rect.Use();
                    Texture.Use();
                    Shader.Use();

                    GL.DrawElements(PrimitiveType.Triangles, Rect.indices.Length, DrawElementsType.UnsignedInt, 0);
                    GL.Viewport(0, 0, Context2D.WindowSize.X, Context2D.WindowSize.Y);
                }
            }
        }
    }
}