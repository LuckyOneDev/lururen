namespace Lururen.Core.ResourceLoader.DllResourceLoader
{
    /// <summary>
    /// Class to create loader. Can be used if there will ever be a need of more complex steps
    /// </summary>
    public class DllResourceLoaderBuilder
    {
        private readonly List<string> dllPaths = new();
        public void AddDllToBeLoaded(string fullPath)
        {
            dllPaths.Add(fullPath);
        }
        public void AddDllToBeLoaded(string[] paths)
        {
            dllPaths.AddRange(paths);
        }
        public DllResourceLoader Build()
        {
            DllResourceLoader loader = new();
            // Can be more steps here
            loader.LoadDll(dllPaths.ToArray());
            return loader;
        }
    }
}