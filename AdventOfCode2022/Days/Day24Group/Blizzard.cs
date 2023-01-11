using System.Drawing;

namespace AdventOfCode2022.Days.Day24Group
{
    public record Blizzard
    {
        public Point Location { get; set; }
        public char Direction { get; set; }
        public Blizzard LocationAtStep(int step, int width, int height)
        {
            var point = Direction switch
            {
                '>' => Location with { X = (Location.X + step) % width },
                '<' => Location with { X = (Location.X - step) % width },
                'v' => Location with { Y = (Location.Y + step) % height },
                '^' => Location with { Y = (Location.Y - step) % height },
                _ => throw new InvalidOperationException("Unknown direction")
            };

            if (point.X < 0)
            {
                point.X += width;
            }
            if (point.Y < 0)
            {
                point.Y += height;
            }    

            return this with
            {
                Location = point
            };

        }
    }
}