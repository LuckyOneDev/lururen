using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace lururen.Common
{
    // System.Numerics.Vector3 ?????
    internal struct ScalarPoint
    {
        public ScalarPoint(int x = 0, int y = 0, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}
