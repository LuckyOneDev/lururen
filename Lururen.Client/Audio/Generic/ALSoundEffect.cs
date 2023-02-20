using Lururen.Client.Graphics.Generic;
using Lururen.Client.ResourceManagement;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
using System.Collections;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Threading.Channels;

namespace Lururen.Client.Audio.Generic
{
    public class ALSoundEffect : IByteConstructable<ALSoundEffect>
    {
        public ALSoundEffect(byte[] bytes, int sampleRate, ALFormat format)
        {
            this.Data = bytes;
            this.SampleRate = sampleRate;
            this.Format = format;
            this.Handle = OpenALHelper.InitBuffer(bytes, format, sampleRate);
        }

        protected static void ResamplePcm(Stream waveFileReader, out byte[] buffer, int blockAlign, int channels)
        {
            const int targetByteSize = 2;

            var bytesPerChannel = blockAlign / channels;

            buffer = new byte[waveFileReader.Length * targetByteSize / bytesPerChannel];

            var sampleBuffer = new byte[blockAlign];

            // Read block by block and resample
            for (int i = 0; waveFileReader.Read(sampleBuffer, 0, blockAlign) != 0; i++)
            {
                // Go over every channel
                for (int j = 0; j < channels; j++)
                {
                    // Get channel data 
                    var channelSample = new byte[targetByteSize];
                    Array.Copy(sampleBuffer, j * (bytesPerChannel - 1), channelSample, 0, targetByteSize);
                    channelSample.CopyTo(buffer, targetByteSize * i + j);
                }
                sampleBuffer = new byte[bytesPerChannel * blockAlign];
            }
        }

        public static byte[] LoadWave(Stream stream, out ALFormat soundFormat, out int sampleRate)
        {
            using (WaveFileReader waveFileReader = new WaveFileReader(stream))
            {
                var encoding = waveFileReader.WaveFormat.Encoding;

                if (encoding != WaveFormatEncoding.Pcm) throw new NotSupportedException("Audio format not supported");

                sampleRate = waveFileReader.WaveFormat.SampleRate;

                soundFormat = GetSoundFormat(waveFileReader.WaveFormat.Channels, waveFileReader.WaveFormat.BitsPerSample);

                // Resampling to 16 bit format
                if (waveFileReader.WaveFormat.BitsPerSample > 16)
                {
                    ResamplePcm(waveFileReader, out byte[] buffer, waveFileReader.WaveFormat.BlockAlign, waveFileReader.WaveFormat.Channels);
                    return buffer;
                } 
                else
                {
                    var buffer = new byte[waveFileReader.Length];
                    waveFileReader.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
        }

        public static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public static ALSoundEffect FromBytes(Stream byteStream, FileAccessor accessor)
        {
            byte[] bytes = null;
            int sampleRate = 0;
            ALFormat format;

            var fileExtension = accessor.Path.Split('.').Last();

            switch (fileExtension)
            {
                case "wav":
                    bytes = LoadWave(byteStream, out format, out sampleRate);
                    break;
                default:
                    throw new NotSupportedException($"Audio file format {fileExtension} not supported");
            }

            return new ALSoundEffect(bytes.ToArray(), sampleRate, format);
        }

        public ALFormat Format { get; }
        public int Handle { get; }
        public byte[] Data { get; }
        public int SampleRate { get; }
    }
}
