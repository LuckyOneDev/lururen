using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Common
{
    public static class EmbededResourceHelper
    {
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }

        }

        public static byte[] ReadBytes(Assembly asm, string path)
        {
            var stream = GetStream(asm, path);
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadAllBytes();
        }

        public static string ReadString(Assembly asm, string path)
        {
            var stream = GetStream(asm, path);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static Stream GetStream(Assembly asm, string path)
        {
            var stream = asm.GetManifestResourceStream(path);
            if (stream == null)
            {
                throw new Exception($"Could not read embeded resource. Path: {path}");
            }
            return stream;
        }
    }
}
