namespace Lururen.Client.Graphics
{
    public class ResourceHandle
    {
        private static Dictionary<string, ResourceHandle> resourceHandles = new();
        public string FilePath { get; private set; }
        private byte[] Bitmap { get; set; }
        private ResourceHandle(string filePath)
        {
            FilePath = filePath;
            resourceHandles.Add(filePath, this);
        }

        public byte[] GetBytes()
        {
            if (Bitmap is null)
            {
                Bitmap = File.ReadAllBytes(FilePath);
            }

            return Bitmap;
        }

        public static ResourceHandle Get(string filePath)
        {
            if (resourceHandles.ContainsKey(filePath))
            {
                return resourceHandles[filePath];
            }
            else
            {
                return new ResourceHandle(filePath);
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