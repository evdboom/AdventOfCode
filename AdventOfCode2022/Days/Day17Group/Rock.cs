using System.Drawing;

namespace AdventOfCode2022.Days.Day17Group
{
    public record Rock
    {
        public List<Point> Points { get; } = new();
        public int StopsAt { get; set; }
        public int Left { get; set; }
        public int Height => Points
            .Select(p => p.Y + 1)
            .Max();
        public int Width => Points
            .Select(p => p.X + 1)
            .Max();

        public IEnumerable<Point> Absolute => Points
            .Select(point => new Point(point.X + Left, point.Y + StopsAt));

        public Rock(params Point[] points)
        {
            Points.AddRange(points);
        }
    }
}
