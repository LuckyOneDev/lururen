using Lururen.Common.EntitySystem;
using Lururen.Common.EventSystem;
using Lururen.Common.Models;
using Lururen.Server.App;

namespace Lururen.Server.EntitySystem
{
    /// <summary>
    /// Entity is any object that should contain data and/or logic of in game objects.
    /// E.g. characters, buttons, tiles.
    /// </summary>
    public abstract class ServerEntity : IEntity
    {
        public bool IsInitialized { get; protected set; } = false;
        /// <summary>
        /// Called by system to register entity in engine
        /// </summary>
        public void SysInit(Application appInstance)
        {
            // Connection to event bus depending on Subscribed events list
            SubscribedEvents.ForEach(evt =>
            {
                appInstance.EventBus.Subscribe(evt.GetType(), this);
            });

            // Controller initialization logic
            if (Controller != null) { Controller.Parent = this; }

            // In case we want to reinit entity. Init is only called once still.
            if (!IsInitialized)
            {
                Init();
            }
            IsInitialized = true;
        }
        public abstract void Init();
        public abstract void Update(double deltaTime);
        public abstract void Dispose();
        public abstract void OnEvent(EventArgs args);
        public List<IEvent> SubscribedEvents { get; set; } = new();
        public IEntityController? Controller { get; set; }
    }
}