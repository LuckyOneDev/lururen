namespace Lururen.Core.EntitySystem
{
    public interface IEntityController
    {
        public Entity Parent { get; set; }
        public abstract void Init();
        public abstract void Update();
        public abstract void Dispose();
    }
}
