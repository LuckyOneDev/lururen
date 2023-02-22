using OpenTK.Audio.OpenAL;

namespace Lururen.Client.Audio
{
    public static class OpenALHelper
    {
        public static int InitBuffer<T>(T[] data, ALFormat format, int freq) where T : unmanaged
        {
            int buffer = AL.GenBuffer();
            AL.BufferData(buffer, format, data, freq);
            CheckAlError();
            return buffer;
        }

        public static void CheckAlError()
        {
            var error = AL.GetError();
            if (error != ALError.NoError)
            {
                // Something went wrong
                throw new OpenALException($"OpenAL call failed. Error: {error}");
            }
        }
    }
}
