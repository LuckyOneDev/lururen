using Lururen.Client.ResourceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Audio.Generic
{
    public class SoundSource
    {
        public ALSoundSoruce ALSoundSource = new();
        public Sound Sound { get; set; } = null;
        public SoundSource()
        {

        }

        public async Task Play(Sound sound)
        {
            this.Sound = Sound;
            var soundEffect = FileHandle<ALSoundEffect>.GetInstance().Get(sound.Accessor);
            await ALSoundSource.Play(soundEffect);
        }
    }
}
