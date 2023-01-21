namespace Lururen.Core.Common
{
    /// <summary>
    /// Scalar vector implementation
    /// </summary>
    public struct SVector3
    {
        public SVector3(int x = 0, int y = 0, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static SVector3 Zero => new(0, 0, 0);
        public static SVector3 One => new(1, 1, 1);

        /// <summary>
        /// Calculates geometrical distance (shortest path) between two scalar points defined by vectors a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(SVector3 a, SVector3 b)
        {
            return Math.Cbrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z));
        }

        /// <summary>
        /// Calculates manhattan distance (rectilinear distance) between two scalar points defined by vectors a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ManhattanDistance(SVector3 a, SVector3 b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
        }

        /// <summary>
        /// Calculates sum of two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SVector3 Sum(SVector3 a, SVector3 b)
        {
            return new SVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Calculates substraction of two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SVector3 Sub(SVector3 a, SVector3 b)
        {
            return new SVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
    }
}
