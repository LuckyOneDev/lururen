using Lururen.Client.ResourceManagement;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;

namespace Lururen.Client.Audio.Generic
{
    /// <summary>
    /// OpenAL backend for SoundEffect.
    /// </summary>
    public class ALSoundEffect : IByteConstructable<ALSoundEffect>
    {
        public ALSoundEffect(byte[] bytes, int sampleRate, ALFormat format)
        {
            this.Data = bytes;
            this.SampleRate = sampleRate;
            this.Format = format;
            this.Handle = OpenALHelper.InitBuffer(bytes, format, sampleRate);
        }

        /// <summary>
        /// Gets raw PCM data.
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// Gets OpenAL sound format.
        /// </summary>
        public ALFormat Format { get; }

        /// <summary>
        /// Gets OpenAL buffer handle.
        /// </summary>
        public int Handle { get; }

        /// <summary>
        /// Gets sample rate.
        /// </summary>
        public int SampleRate { get; }

        public static ALSoundEffect FromBytes(Stream byteStream, FileAccessor accessor)
        {
            byte[] bytes;
            int sampleRate;
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

            return new ALSoundEffect(bytes, sampleRate, format);
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

        /// <summary>
        /// Reads wav file to PCM byte array.
        /// </summary>
        /// <param name="stream">Filestream</param>
        /// <param name="soundFormat">resulting sound format</param>
        /// <param name="sampleRate">resulting sample rate</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
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

                // Resampling to float32 format
                if (waveFileReader.WaveFormat.BitsPerSample > 16)
                {
                    soundFormat = waveFileReader.WaveFormat.Channels > 1 ? ALFormat.StereoFloat32Ext : ALFormat.StereoFloat32Ext;
                    buffer = ResampleToFloat32(buffer, waveFileReader.WaveFormat.BlockAlign, waveFileReader.WaveFormat.Channels);
                }

                return buffer;
            }
        }

        /// <summary>
        /// Resamples PCM format from int24 to float32.
        /// </summary>
        /// <param name="buffer">int24 PCM data</param>
        /// <param name="blockAlign">data sample block align</param>
        /// <param name="channels">channels count</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        protected static byte[] ResampleToFloat32(byte[] buffer, int blockAlign, int channels)
        {
            int channelBytes = blockAlign / channels;
            var outBuffer = new byte[buffer.Length * sizeof(float) / channelBytes];

            for (int i = 0; i < outBuffer.Length; i += sizeof(float))
            {
                var subBuffer = new byte[4];

                if (BitConverter.IsLittleEndian)
                {
                    Array.Copy(buffer, i / sizeof(float) * channelBytes, subBuffer, 1, channelBytes);
                }
                else
                {
                    throw new NotSupportedException("Big endian processors not supported.");
                }

                float fSample = BitConverter.ToInt32(subBuffer, 0) / (float)uint.MaxValue;
                if (fSample > 1) fSample = 1;
                if (fSample < -1) fSample = -1;

                Array.Copy(BitConverter.GetBytes(fSample), 0, outBuffer, i, sizeof(float));
            }

            return outBuffer;
        }
    }
}