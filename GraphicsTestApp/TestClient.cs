using OpenTK.Windowing.GraphicsLibraryFramework;
using ResourceLocation = Lururen.Client.ResourceManagement.ResourceLocation;
using Lururen.Client.Graphics.Texturing;
using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Components;
using Lururen.Client.EntityComponentSystem.Base;
using OpenTK.Mathematics;
using System.Numerics;

namespace GraphicsTestApp
{
    public static class PrefabCollection
    {
        public static Prefab Player => new Prefab((Entity ent) =>
        {
            var sc = ent.AddComponent<SpriteComponent>();
            sc.Texture = new Texture2D("GraphicsTestApp.megumin.png", ResourceLocation.Embeded);
            ent.AddComponent<Camera>();
            ent.AddComponent<PlayerControl>();
        });
    }

    public class TestClient : Application2D
    {
        protected override void Init()
        {
            base.Init();
            var player = Instantiate(PrefabCollection.Player);
        }
    }
}