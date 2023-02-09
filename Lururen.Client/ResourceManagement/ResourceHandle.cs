namespace Lururen.Client.ResourceManagement
{
    public abstract class ResourceHandle<Value, Accessor> where Accessor : notnull
    {
        public Value Get(Accessor accessor)
        {
            var loaded = GetResourceLoaded(accessor);
            if (!loaded)
            {
                LoadResource(accessor);
            }
            return GetResource(accessor);
        }

        public abstract Dictionary<Accessor, Value> GetLoaded();

        public abstract bool GetResourceLoaded(Accessor acessor);

        protected abstract Value GetResource(Accessor accessor);

        protected abstract void LoadResource(Accessor acessor);

        protected abstract void UnloadResource(Accessor acessor);
    }
}