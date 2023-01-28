using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Common
{
    public static class ResourceHelper
    {
        public static string ReadEmbededResource(Assembly asm, string path)
        {
            var stream = GetEmbededResourceStream(asm, path);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static Stream GetEmbededResourceStream(Assembly asm, string path)
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
