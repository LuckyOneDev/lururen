using Lururen.Client.EntityComponentSystem;
using Lururen.Client.EntityComponentSystem.Planar;
using Lururen.Client.ResourceManagement;
using OpenTK.Mathematics;

namespace Lururen.Client.Audio.Generic
{
    public record SoundPlayProperties
    {
        public bool Looping = false;
        public int Pitch = 1;
        public int Gain = 0;
        public float Speed = 1;
    }

    public class SoundSource : Component
    {
        internal ALSoundSoruce ALSoundSource = new();
        public bool IsPlaying { get; protected set; } = false;
        public SoundSource(Entity entity) : base(entity)
        {
        }

        public Sound? CurrentSound { get; set; }
        public SoundPlayProperties? properties { get; set; }

        public async Task Play(Sound sound, SoundPlayProperties properties = default)
        {
            this.CurrentSound = CurrentSound;
            var soundEffect = FileHandle<ALSoundEffect>.GetInstance().Get(sound.Accessor);
            do
            {
                IsPlaying = true;
                await ALSoundSource.Play(soundEffect);
            }
            while (properties.Looping && IsPlaying);
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            ALSoundSource.SetPos(new Vector3(Transform.Position));
        }
    }
}
