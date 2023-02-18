using Lururen.Client.Graphics.Generic;
using Lururen.Client.ResourceManagement;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
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

        public static byte[] LoadWave(Stream stream, out ALFormat soundFormat, out int sampleRate)
        {
            using (WaveFileReader waveFileReader = new WaveFileReader(stream))
            {
                var encoding = waveFileReader.WaveFormat.Encoding;

                if (encoding != WaveFormatEncoding.Pcm) throw new NotSupportedException("Audio format not supported");

                sampleRate = waveFileReader.WaveFormat.SampleRate;

                soundFormat = GetSoundFormat(waveFileReader.WaveFormat.Channels, waveFileReader.WaveFormat.BitsPerSample);
                List<byte> byteBuffer = new();


                for (long i = 0; i < waveFileReader.SampleCount; i++)
                {
                    var frame = waveFileReader.ReadNextSampleFrame();
                    byteBuffer.AddRange(frame.SelectMany(x => BitConverter.GetBytes(x)));
                }

                return byteBuffer.ToArray();
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
