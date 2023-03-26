using Lururen.Client.EntityComponentSystem.Camera;
using Lururen.Client.EntityComponentSystem.Sound;
using Lururen.Client.EntityComponentSystem.Sprite;
using Lururen.Client.EntityComponentSystem.User;

namespace Lururen.Client.Base
{
    public class Application2D : Application
    {
        public Application2D()
        {
            WorldManager.Create("default");
            WorldManager.SetActiveWorld("default");
        }

        protected override void Init()
        {
            base.Init();

            var spriteRender = new SpriteRenderSystem();
            var camSystem = new CameraSystem();
            var soundSystem = new SoundSystem();
            var ucSystem = new UserComponentSystem();

            spriteRender.BindSystems(Window, camSystem);

            SystemManager.RegisterSystem(spriteRender);
            SystemManager.RegisterSystem(camSystem);
            SystemManager.RegisterSystem<SoundSource>(soundSystem);
            SystemManager.RegisterSystem<SoundListener>(soundSystem);
            SystemManager.RegisterSystem(ucSystem);
        }
    }
}