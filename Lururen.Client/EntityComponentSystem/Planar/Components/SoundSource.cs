using Lururen.Client.EntityComponentSystem.Planar;
using Lururen.Client.ResourceManagement;

namespace Lururen.Client.Audio.Generic
{
    public record SoundPlayProperties
    {
        public bool Looping = false;
        public int Pitch = 1;
        public int Gain = 0;
        public float Speed = 1;
    }

    public class SoundSource : Component2D
    {
        internal ALSoundSoruce ALSoundSource = new();
        public bool IsPlaying { get; protected set; } = false;
        public SoundSource(Entity2D entity) : base(entity)
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
    }
}
