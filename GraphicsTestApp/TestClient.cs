using OpenTK.Windowing.GraphicsLibraryFramework;
using ResourceLocation = Lururen.Client.ResourceManagement.ResourceLocation;
using Lururen.Client.Graphics.Texturing;
using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Base;
using OpenTK.Mathematics;
using System.Numerics;
using Vector2 = OpenTK.Mathematics.Vector2;
using Lururen.Client.EntityComponentSystem.Camera;
using Lururen.Client.EntityComponentSystem.Sprite;

namespace GraphicsTestApp
{
    public static class PrefabCollection
    {
        public static Prefab Camera => new Prefab((Entity ent) =>
        {
            ent.AddComponent<Camera>();
        });

        public static Prefab Player => new Prefab((Entity ent) =>
        {
            var sc = ent.AddComponent<SpriteComponent>();
            sc.Texture = new Texture2D("GraphicsTestApp.megumin.png", ResourceLocation.Embeded);
            sc.Pivot = new Vector2(0.5f, 0.5f);
            ent.AddComponent<PlayerControl>();
        });

        public static Prefab Wall => new Prefab((Entity ent) =>
        {
            var sc = ent.AddComponent<SpriteComponent>();
            sc.Texture = new Texture2D("GraphicsTestApp.wall.jpg", ResourceLocation.Embeded);
        });
    }

    public class TestClient : Application2D
    {
        protected override void Init()
        {
            base.Init();
            var player = Instantiate(PrefabCollection.Player);
            var ca = Instantiate(PrefabCollection.Camera);
            for (int i = 0; i < 10; i++)
            {
                var a = Instantiate(PrefabCollection.Wall, i, i % 2, -1);
            }
        }
    }
}