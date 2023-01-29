using Lururen.Common;
using System.Reflection;

namespace Lururen.Client
{
    public enum ResourceLocation
    {
        FileSystem = 0,
        Embeded = 1,
    }

    /// <summary>
    /// Ensures file is loaded into memory only once
    /// </summary>
    public class ResourceHandle
    {
        private static Dictionary<string, ResourceHandle> resourceHandles = new();
        public string FilePath { get; private set; }
        private byte[] Bitmap { get; set; }
        public ResourceLocation ResourceLocation { get; }
        private ResourceHandle(string filePath, ResourceLocation resourceLocation)
        {
            FilePath = filePath;
            resourceHandles.Add(filePath, this);
            ResourceLocation = resourceLocation;
            Bitmap = GetBytes();
        }

        public void EnsureLoaded()
        {
            if (Bitmap is null)
            {
                switch (ResourceLocation)
                {
                    case ResourceLocation.FileSystem:
                        Bitmap = File.ReadAllBytes(FilePath);
                        break;
                    case ResourceLocation.Embeded:
                        Bitmap = EmbededResourceHelper.ReadBytes(Assembly.GetEntryAssembly(), FilePath);
                        break;
                }
            }
        }

        public byte[] GetBytes()
        {
            EnsureLoaded();
            return Bitmap;
        }

        public Stream GetStream()
        {
            EnsureLoaded();
            return new MemoryStream(Bitmap);
        }

        public static ResourceHandle Get(string filePath, ResourceLocation resourceLocation = ResourceLocation.FileSystem)
        {
            if (resourceHandles.ContainsKey(filePath))
            {
                return resourceHandles[filePath];
            }
            else
            {
                return new ResourceHandle(filePath, resourceLocation);
            }
        }

        public static void Unload(ResourceHandle handle)
        {
            resourceHandles.Remove(handle.FilePath);
        }

        public static void Unload(string filePath)
        {
            resourceHandles.Remove(filePath);
        }


    }
}