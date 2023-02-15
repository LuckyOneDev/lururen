using Lururen.Client.ResourceManagement;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lururen.Client.Audio.Generic
{
    public class ALSoundBuffer
    {
        List<int> SoundEffectBuffers { get; set; } = new();

        public unsafe void AddSoundEffect(ALFormat format, Stream stream)
        {
            var audioFileReader = new Mp3FileReader(stream);
            var frame = audioFileReader.ReadNextFrame();
            var rawData = frame.RawData;

            int buffer = AL.GenBuffer();
            SoundEffectBuffers.Add(buffer);

            var soundData = new short[11];
            
            AL.BufferData(buffer, format, soundData, rawData.Length * sizeof(byte), frame.SampleRate);
        }

        public bool RemoveSoundEffect(int handle)
        {
            SoundEffectBuffers.Remove(handle);
            AL.DeleteBuffer(handle);
        }
    }
}
