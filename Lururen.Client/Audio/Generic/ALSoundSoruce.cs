using OpenTK.Audio.OpenAL;
using OpenTK.Compute.OpenCL;
using OpenTK.Mathematics;

namespace Lururen.Client.Audio.Generic
{
    public class ALSoundSoruce : IDisposable
    {
        private int sourceHandle;

        public ALSoundSoruce(Vector3 position, Vector3 direction, Vector3 velocity, float pitch, float gain)
        {
            this.sourceHandle = AL.GenSource();
            AL.Source(sourceHandle, ALSourcei.Buffer, 0);

            AL.Source(sourceHandle, ALSourcef.Pitch, pitch);
            AL.Source(sourceHandle, ALSourcef.Gain, gain);
            AL.Source(sourceHandle, ALSource3f.Position, position.X, position.Y, position.Z);
            AL.Source(sourceHandle, ALSource3f.Direction, direction.X, direction.Y, direction.Z);
            AL.Source(sourceHandle, ALSource3f.Velocity, velocity.X, velocity.Y, velocity.Z);
        }

        public void Play(int buffer)
        {
            /*
            if (buffer_to_play != p_Buffer)
            {
                p_Buffer = buffer_to_play;
                alSourcei(p_Source, AL_BUFFER, (ALint)p_Buffer);
            }

            alSourcePlay(p_Source);


            ALint state = AL_PLAYING;
            std::cout << "playing sound\n";
            while (state == AL_PLAYING && alGetError() == AL_NO_ERROR)
            {
                std::cout << "currently playing sound\n";
                alGetSourcei(p_Source, AL_SOURCE_STATE, &state);
            }
            std::cout << "done playing sound\n";
            */
        }

        public void Dispose()
        {
            AL.DeleteSource(sourceHandle);
        }
    }
}
