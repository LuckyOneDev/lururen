using Lururen.Client.Audio.Generic;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.EntityComponentSystem.Planar.Systems
{
    public class SoundSystem : ISoundSystem<SoundSource>, ISoundSystem<Camera2D>
    {
        List<SoundSource> Sources { get; set; } = new();
        List<ALSoundDevice> Devices { get; set; } = new();
        
        #region Singleton
        private static SoundSystem instance;

        private SoundSystem() { }

        public static SoundSystem GetInstance()
        {
            if (instance == null)
                instance = new SoundSystem();
            return instance;
        }
        #endregion

        public void Init()
        {
        }

        public void Register(SoundSource component)
        {
            Sources.Add(component);
        }

        public void Register(Camera2D component)
        {
            Devices.Add(component.SoundDevice);
        }

        public void Unregister(SoundSource component)
        {
            Sources.Remove(component);
        }

        public void Unregister(Camera2D component)
        {
            Devices.Remove(component.SoundDevice);
        }

        public void Update(double deltaTime)
        {
            foreach (var item in Sources)
            {
                item.Update(deltaTime);
            }
        }
    }
}
