﻿namespace Lururen.Client.EntityComponentSystem.Sound
{
    using Lururen.Client.Audio;
    using Lururen.Client.Audio.Generic;
    using Lururen.Client.EntityComponentSystem.Base;
    using Lururen.Client.ResourceManagement;
    using OpenTK.Mathematics;

    public record SoundPlayProperties
    {
        public bool Looping = false;
        public int Pitch = 1;
        public int Gain = 0;
        public float Speed = 1;
    }

    public sealed class SoundSource : Component
    {
        internal ALSoundSoruce ALSoundSource = new();
        public bool IsPlaying { get; protected set; } = false;
        public SoundSource(Entity entity) : base(entity)
        {
            Register(this);
        }

        public Sound? CurrentSound { get; set; }
        public SoundPlayProperties? properties { get; set; }

        public async Task Play(Sound sound, SoundPlayProperties properties = default)
        {
            CurrentSound = CurrentSound;
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
            ALSoundSource.SetPos(new Vector3(Transform.Position));
        }

        public override void Dispose()
        {
            Register(this);
            ALSoundSource.Dispose();
            base.Dispose();
        }

        public override void Init()
        {
        }
    }
}
