﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.Common.Protocol
{
    public static class ProtocolHelper
    {
        public static byte GetChecksum(byte[] data)
        {
            byte sum = 0;
            // Let overflow occur without exceptions
            unchecked
            {
                foreach (byte b in data)
                {
                    sum += b;
                }
            }
            return sum;
        }
    }
}