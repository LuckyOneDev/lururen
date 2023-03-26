using Lururen.Client.Audio.Generic;
using Lururen.Client.Base;
using Lururen.Client.EntityComponentSystem.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.EntityComponentSystem.Sound
{
    public class SoundSystem : ISystem<SoundSource>, ISystem<SoundListener>
    {
        List<SoundSource> Sources { get; set; } = new();
        List<SoundListener> Listeners { get; set; } = new();

        public Application Application { get; private set; }
        public void Init(Application app)
        {
            Application = app;
            Application.Window!.OnUpdate += Update;
        }

        public void Register(SoundSource component)
        {
            Sources.Add(component);
        }

        public void Register(SoundListener component)
        {
            Listeners.Add(component);
        }

        public void Unregister(SoundSource component)
        {
            Sources.Remove(component);
        }

        public void Unregister(SoundListener component)
        {
            Listeners.Remove(component);
        }

        public void Update(double deltaTime)
        {
            foreach (var item in Sources)
            {
                item.Update(deltaTime);
            }
        }

        public void Destroy()
        {
        }
    }
}
