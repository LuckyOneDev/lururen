using OpenTK;
using OpenTK.Audio.OpenAL;

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
            
            if (!ALC.MakeContextCurrent(this.Context))
            {
                // Something went wrong
                throw new Exception($"Audio subsystem could not initialize. Error: {ALC.GetError(this.Device)}");
            }
        }
          
        public void Dispose()
        {
            ALC.DestroyContext(this.Context);
            ALC.CloseDevice(this.Device);
        }
    }
}
