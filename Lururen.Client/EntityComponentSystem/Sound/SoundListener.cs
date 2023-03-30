using Lururen.Client.Audio.Generic;
using Lururen.Client.EntityComponentSystem.Base;
using OpenTK.Mathematics;

namespace Lururen.Client.EntityComponentSystem.Sound
{
    public sealed class SoundListener : Component
    {
        ALSoundDevice SoundDevice = new ALSoundDevice();
        public SoundListener(Entity entity) : base(entity)
        {
        }

        public override void Init()
        {
            SoundDevice.SetPosition(new Vector3(Transform.Position));
        }

        public override void Update(double deltaTime)
        {
            SoundDevice.SetPosition(new Vector3(Transform.Position));
        }
    }
}