using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Lururen.Client.Audio.Generic
{
    public class ALSoundSoruce : IDisposable
    {
        internal int sourceHandle;

        public ALSoundEffect? CurrentSoundEffect = null;
        public ALSourceState State { get; private set; }

        public ALSoundSoruce()
        {
            this.sourceHandle = AL.GenSource();
            //AL.Source(sourceHandle, ALSourcef.Pitch, 1f);
            //AL.Source(sourceHandle, ALSourcef.Gain, 1f);
            //AL.Source(sourceHandle, ALSource3f.Position, 0, 0, 0);
            //AL.Source(sourceHandle, ALSource3f.Direction, 0, 0, 0);
            //AL.Source(sourceHandle, ALSource3f.Velocity, 0, 0, 0);
        }

        public ALSoundSoruce(Vector3 position, Vector3 direction, Vector3 velocity, float pitch, float gain)
        {
            this.sourceHandle = AL.GenSource();
            //AL.Source(sourceHandle, ALSourcef.Pitch, pitch);
            //AL.Source(sourceHandle, ALSourcef.Gain, gain);
            //AL.Source(sourceHandle, ALSource3f.Position, position.X, position.Y, position.Z);
            //AL.Source(sourceHandle, ALSource3f.Direction, direction.X, direction.Y, direction.Z);
            //AL.Source(sourceHandle, ALSource3f.Velocity, velocity.X, velocity.Y, velocity.Z);
        }

        public void SetPos(Vector3 pos)
        {

        }

        public async Task Play(ALSoundEffect sound)
        {
            if (CurrentSoundEffect != sound)
            {
                CurrentSoundEffect = sound;
                AL.Source(sourceHandle, ALSourcei.Buffer, sound.Handle);
            }

            AL.SourcePlay(sourceHandle);

            this.State = ALSourceState.Playing;
            var error = ALError.NoError;

            while (this.State == ALSourceState.Playing && error == ALError.NoError)
            {
                this.State = AL.GetSourceState(sourceHandle);
                error = AL.GetError();
            }

            if (error != ALError.NoError)
            {
                throw new OpenALException($"Could not play sound effect. Error: {error}.");
            }
        }

        public void Dispose()
        {
            AL.DeleteSource(sourceHandle);
        }
    }
}
