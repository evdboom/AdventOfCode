using System.Drawing;

namespace AdventOfCode.Shared.Extensions
{
    public static class PointExtensions
    {
        public static int GetManhattanDistance(this Point source, Point destination)
        {
            return Math.Abs(source.X - destination.X) + Math.Abs(source.Y - destination.Y);
        }
    }
}
