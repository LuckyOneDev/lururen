using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Audio
{
    public class OpenALException : Exception
    {
        public OpenALException(string? message) : base(message)
        {
        }

        public OpenALException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
