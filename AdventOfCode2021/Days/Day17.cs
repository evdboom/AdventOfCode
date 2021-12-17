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

            var minX = GetMinXVelocity(area.X);
            foreach (var y in PotentialY(area))
            {
                foreach (var x in PotentialX(area, minX))
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

            var minX = GetMinXVelocity(area.X);
            foreach (var y in PotentialY(area))
            {
                foreach (var x in PotentialX(area, minX))
                {
                    if (MaxHeight(x, y, area) > long.MinValue)
                    {
                        hits++;
                    }
                }
            }
            
            return hits;
        }

        private int GetMinXVelocity(int xToReach)
        {
            var option = 0;
            while(option < xToReach)
            {
                var max = GetMaxX(option);
                if (max >= xToReach)
                {
                    break;
                }
                option++;
            }

            return option;
        }

        private int GetMaxX(int option)
        {
            var max = 0;
            while (option > 0)
            {
                max += option;
                option--;
            }

            return max;
        }

        private IEnumerable<int> PotentialY(Rectangle area)
        {
            var size = Math.Abs(area.Y) - area.Y + 1;
            return Enumerable.Range(area.Y, size);
           
        }

        private IEnumerable<int> PotentialX(Rectangle area, int start)
        {
            var size = Math.Max(1, area.X + area.Width + 1 - start);
            return Enumerable.Range(start, size);
        }


        private long MaxHeight(int xVelocity, int yVelocity, Rectangle area)
        {
            var point = new Point(0, 0);
            var yMax = 0;
            var hit = false;
            var minStep = 1 + 2 * yVelocity;
            var step = 0;
            while (step < minStep || CouldBeInArea(point, area))
            {
                point = new Point(point.X + xVelocity, point.Y + yVelocity);
                yMax = Math.Max(yMax, point.Y);

                if (step >= minStep && InArea(point, area))
                {
                    hit = true;
                    break;
                }

                step++;
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
