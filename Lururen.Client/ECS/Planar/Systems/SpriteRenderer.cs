using Lururen.Client.ECS.Planar.Components;
using Lururen.Client.Graphics;
using Lururen.Client.Graphics.Generic;
using OpenTK.Mathematics;

namespace Lururen.Client.ECS.Planar.Systems
{
    public class SpriteRenderer : BaseSystem<Sprite>
    {
        private Dictionary<string, Shader> LoadedShaders { get; set; } = new Dictionary<string, Shader>();
        private Dictionary<string, Texture> LoadedTextures { get; set; } = new Dictionary<string, Texture>();

        public Texture GetTexture(string path)
        {
            return LoadedTextures[path];
        }

        public Shader GetShader(string path)
        {
            return LoadedShaders[path];
        }

        protected static Game Window { get; set; }

        public void Init(Game window)
        {
            Window = window;
        }

        public static Vector2i WindowSize => Window.ClientSize;
    }
}