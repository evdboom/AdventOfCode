using AdventOfCode2021.Services;
using System.Drawing;

namespace AdventOfCode2021.Days
{
    public class Day17 : Day
    {
        public Day17(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 17;

        protected override long ProcessPartOne(string[] input)
        {
            var area = GetArea(input[0]);
            long yMax = 0;

            foreach(var x in PotentialX(area))
            {
                foreach(var y in PotentialY(area))
                {             
                    yMax = Math.Max(yMax, MaxHeight(x, y, area));
                }
            }
            
            return yMax;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var area = GetArea(input[0]);
            long hits = 0;

            foreach (var x in PotentialX(area))
            {
                foreach (var y in PotentialY(area))
                {
                    if (MaxHeight(x, y, area) > long.MinValue)
                    {
                        hits++;
                    }
                }
            }
            
            return hits;
        }

        private IEnumerable<int> PotentialY(Rectangle area)
        {
            var size = Math.Abs(area.Y) - area.Y + 1;
            return Enumerable.Range(area.Y, size);
           
        }

        private IEnumerable<int> PotentialX(Rectangle area)
        {
            return Enumerable.Range(0, area.X + area.Width + 1);
        }


        private long MaxHeight(int xVelocity, int yVelocity, Rectangle area)
        {
            var point = new Point(0, 0);
            var yMax = 0;
            var hit = false;
            while (CouldBeInArea(point, area))
            {
                point = new Point(point.X + xVelocity, point.Y + yVelocity);
                yMax = Math.Max(yMax, point.Y);

                if (InArea(point, area))
                {
                    hit = true;
                    break;
                }

                xVelocity += xVelocity < 0 ? 1
                           : xVelocity > 0 ? -1
                           : 0;                
                yVelocity--;
            }

            return hit
                ? yMax
                : long.MinValue;            
        }

        private bool InArea(Point point, Rectangle area)
        {
            return 
                point.X >= area.X &&
                point.X <= area.X + area.Width &&
                point.Y >= area.Y &&
                point.Y <= area.Y + area.Height;
        }

        private bool CouldBeInArea(Point point, Rectangle area)
        {
            return 
                point.X <= area.X + area.Width &&
                point.Y >= area.Y;
        }

        private Rectangle GetArea(string line)
        {
            var values = line
                .Substring(line.IndexOf('x'))
                .Replace("x=", "")
                .Replace("y=", "")
                .Split(',')
                .SelectMany(v => v.Split(".."))
                .Select(v => int.Parse(v))
                .ToArray();

            var minX = Math.Min(values[0], values[1]);
            var maxX = Math.Max(values[0], values[1]);
            var minY = Math.Min(values[2], values[3]);
            var maxY = Math.Max(values[2], values[3]);

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
