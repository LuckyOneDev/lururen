using Lururen.Client.Graphics.Generic;
using Lururen.Client.ResourceManagement;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        protected static byte[] ResampleToInt16(byte[] buffer, int blockAlign, int channels)
        {
            const int i16Size = 2;
            int channelBytes = blockAlign / channels;
            var outBuffer = new byte[buffer.Length * i16Size / channelBytes];

            for (int i = 0; i < outBuffer.Length; i += i16Size)
            {
                Array.Copy(buffer, i / i16Size * channelBytes, outBuffer, i, i16Size);
            }

            return outBuffer;
        }

        protected static byte[] ResampleToFloat32(byte[] buffer, int blockAlign, int channels)
        {
            const int TargetSize = sizeof(float);
            int channelBytes = blockAlign / channels;
            var outBuffer = new byte[buffer.Length * TargetSize / channelBytes];

            for (int i = 0; i < outBuffer.Length; i += TargetSize)
            {
                var subBuffer = new byte[4];

                if (BitConverter.IsLittleEndian)
                {
                    // subBuffer[0] is the least significant byte
                    Array.Copy(buffer, i / TargetSize * channelBytes, subBuffer, 1, channelBytes);
                } 
                else
                {
                    throw new NotSupportedException("Big endian processors not supported.");
                }

                float fSample = BitConverter.ToInt32(subBuffer, 0) / (float)uint.MaxValue;
                if (fSample > 1) fSample = 1;
                if (fSample < -1) fSample = -1;

                Array.Copy(BitConverter.GetBytes(fSample), 0, outBuffer, i, TargetSize);
            }

            return outBuffer;
        }

        public static byte[] LoadWave(Stream stream, out ALFormat soundFormat, out int sampleRate)
        {
            using (WaveFileReader waveFileReader = new WaveFileReader(stream))
            {
                var encoding = waveFileReader.WaveFormat.Encoding;

                if (encoding != WaveFormatEncoding.Pcm) throw new NotSupportedException("Audio format not supported");

                sampleRate = waveFileReader.WaveFormat.SampleRate;

                soundFormat = GetSoundFormat(waveFileReader.WaveFormat.Channels, waveFileReader.WaveFormat.BitsPerSample);

                var buffer = new byte[waveFileReader.Length];
                waveFileReader.Read(buffer, 0, buffer.Length);

                // Resampling to 16 bit format
                if (waveFileReader.WaveFormat.BitsPerSample > 16)
                {
                    soundFormat = waveFileReader.WaveFormat.Channels > 1 ? ALFormat.StereoFloat32Ext : ALFormat.StereoFloat32Ext;
                    buffer = ResampleToFloat32(buffer, waveFileReader.WaveFormat.BlockAlign, waveFileReader.WaveFormat.Channels);
                    return buffer;
                } 
                else
                {
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
