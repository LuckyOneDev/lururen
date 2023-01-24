using Lururen.Server.ResourceLoader;
using System.Reflection;
namespace Lururen.Server.ResourceLoader.DllResourceLoader
{
    /// <summary>
    /// A simple loader from dll assembly file.
    /// Needs real cases to be thoroughly tested.
    /// Implies that all base classes of class are in the same or host's assembly
    /// </summary>
    public class DllResourceLoader : IResourceLoader
    {
        private readonly List<Assembly> _externalAssemblies = new();
        public DllResourceLoader()
        {

        }

        public void LoadDll(string dllName)
        {
            _externalAssemblies.Add(Assembly.LoadFile(dllName));
        }
        public void LoadDll(string[] dllName)
        {
            _externalAssemblies.AddRange(dllName.Select(Assembly.LoadFile));
        }
        public T LoadChildOf<T>(string concreteChildName)
        {
            if (!_externalAssemblies.Any())
            {
                throw new InvalidOperationException("No assembly was loaded");
            }

            Type? externalExpectedType = _externalAssemblies.Select(a => a
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(T)))
                .FirstOrDefault(type => type.ToString() == concreteChildName)).FirstOrDefault();

            if (externalExpectedType is not Type targetType)
            {
                throw new TypeLoadException($"The assemblies \"{string.Join('\n', _externalAssemblies)}\" does not contain child class {concreteChildName}");
            }

            if (Activator.CreateInstance(targetType) is not T resultingObject)
            {
                throw new InvalidCastException($"The created instance of {typeof(T)} could not be constructed or cast to {targetType}");
            }

            return resultingObject;
        }

        T IResourceLoader.LoadStaticMethod<T>(string methodName)
        {
            throw new NotImplementedException();
        }
    }
}