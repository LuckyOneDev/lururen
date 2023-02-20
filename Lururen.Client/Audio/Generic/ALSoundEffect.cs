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

        //protected void ResamplePcm(byte[] inBytes, out byte[] outBytes, int blockAlign, int channels)
        //{

        //}

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
                    var bytesPerChannel = waveFileReader.WaveFormat.BlockAlign / waveFileReader.WaveFormat.Channels;

                    var buffer = new byte[waveFileReader.Length * 2 / bytesPerChannel];

                    var sampleBuffer = new byte[waveFileReader.WaveFormat.BlockAlign];
                    
                    // Read block by block and resample
                    for (int i = 0; waveFileReader.Read(sampleBuffer, 0, waveFileReader.WaveFormat.BlockAlign) != 0; i ++)
                    {
                        // Go over every channel
                        for (int j = 0; j < waveFileReader.WaveFormat.Channels; j++)
                        {
                            // Get channel data 
                            var channelSample = new byte[2];
                            Array.Copy(sampleBuffer, j * (bytesPerChannel - 1), channelSample, 0, 2);
                            channelSample.CopyTo(buffer, 2 * i + j);
                        }
                        sampleBuffer = new byte[bytesPerChannel * waveFileReader.WaveFormat.BlockAlign];
                    }

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
