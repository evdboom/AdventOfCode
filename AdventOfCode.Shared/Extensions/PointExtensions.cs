using System.Drawing;

namespace AdventOfCode.Shared.Extensions
{
    public static class PointExtensions
    {
        public static int GetManhattanDistance(this Point source, Point destination)
        {
            return Math.Abs(source.X - destination.X) + Math.Abs(source.Y - destination.Y);
        }

        public static double GetEuclideanDistance(this Point source, Point destination)
        {
            return Math.Sqrt(
                Math.Pow(source.X - destination.X, 2) + Math.Pow(source.Y - destination.Y, 2)
            );
        }

        public static double GetEuclideanDistance(this Point3D source, Point3D destination)
        {
            return Math.Sqrt(
                Math.Pow(source.X - destination.X, 2)
                    + Math.Pow(source.Y - destination.Y, 2)
                    + Math.Pow(source.Z - destination.Z, 2)
            );
        }
    }
}
