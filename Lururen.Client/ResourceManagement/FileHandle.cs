using Lururen.Common;
using System.Reflection;

namespace Lururen.Client
{
    public record class FileAccessor
    {
        public string Path { get; init; }
        public ResourceLocation ResourceLocation { get; init; }
    }
    public enum ResourceLocation
    {
        FileSystem = 0,
        Embeded = 1,
    }

    public interface IByteConstructable<Value>
    {
        public static abstract Value FromBytes(byte[] bytes);
    }

    public class FileHandle<T> : ResourceHandle<T, FileAccessor> where T : IByteConstructable<T>
    {

        #region Singleton
        private static FileHandle<T> instance;

        private FileHandle() { }

        public static FileHandle<T> GetInstance()
        {
            if (instance == null)
                instance = new FileHandle<T>();
            return instance;
        }
        #endregion

        protected static Dictionary<FileAccessor, T> Loaded = new();

        public override bool GetResourceLoaded(FileAccessor acessor)
        {
            return Loaded.ContainsKey(acessor);
        }

        protected override T GetResource(FileAccessor accessor)
        {
            return Loaded[accessor];
        }

        protected override void LoadResource(FileAccessor acessor)
        {
            byte[] bytes = null;
            switch (acessor.ResourceLocation)
            {
                case ResourceLocation.FileSystem:
                    bytes = File.ReadAllBytes(acessor.Path);
                    break;

                case ResourceLocation.Embeded:
                    bytes = Assembly.GetEntryAssembly().ReadBytes(acessor.Path);
                    break;
            }

            Loaded.Add(acessor, T.FromBytes(bytes));
        }

        protected override void UnloadResource(FileAccessor acessor)
        {
            Loaded.Remove(acessor);
        }

        public override Dictionary<FileAccessor, T> GetLoaded()
        {
            return Loaded;
        }
    }
}