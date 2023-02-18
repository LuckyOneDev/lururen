using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Lururen.Client.Audio.Generic
{
    public class ALSoundDevice : IDisposable
    {
        public ALDevice Device { get; private set; }
        public ALContext Context { get; private set; }

        public unsafe ALSoundDevice()
        {
            this.Device = ALC.OpenDevice(null);
            this.Context = ALC.CreateContext(this.Device, (int*)null);

            var error = ALC.GetError(this.Device);
            if (!ALC.MakeContextCurrent(this.Context) || error != AlcError.NoError)
            {
                // Something went wrong
                throw new Exception($"Audio subsystem could not be initialized. Error: {error}.");
            }

            AL.Listener(ALListener3f.Position, 0, 0, 0);
            AL.Listener(ALListener3f.Velocity, 0, 0, 0);
        }
          
        public void Dispose()
        {
            ALC.DestroyContext(this.Context);
            ALC.CloseDevice(this.Device);
        }
    }
}
