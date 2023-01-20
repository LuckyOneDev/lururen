using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Core.App
{
    public class ResourceInfo
    {
        public ResourceInfo()
        {
            Resources = new();
        }
        public List<Tuple<string, byte>> Resources { get; set; }
        public void Add(string fileName, byte checkSum)
        {
            Resources.Add(new Tuple<string, byte>(fileName, checkSum));
        }
    }
}
