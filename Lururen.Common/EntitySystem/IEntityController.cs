namespace Lururen.Common.EntitySystem
{
    public interface IEntityController
    {
        public IEntity Parent { get; set; }
        public abstract void Init();
        public abstract void Update();
        public abstract void Dispose();
    }
}
