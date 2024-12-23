using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day20(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 20;
        public int TimeToSave { get; set; } = 100;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid();
            var points = GetCheatlessData(grid);

            return GetCheats(points, TimeToSave, 2);

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid();
            var points = GetCheatlessData(grid);

            return GetCheats(points, TimeToSave, 20);
        }

        private long GetCheats(List<Point> points, int timeToSave, int cheatLength)
        {
            var result = 0L;
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    var distance = points[i].GetManhattanDistance(points[j]);
                    if (j + distance >= i || distance > cheatLength)
                    {
                        continue;
                    }
                    else if (i - (j + distance) >= timeToSave)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        private List<Point> GetCheatlessData(Grid<char> grid)
        {
            var start = grid.First(c => c.Value == 'S').Point;
            var position = grid.First(c => c.Value == 'E');
            var previous = position.Point;
            var path = new List<Point>
            {
                position.Point
            };

            while (position.Point != start)
            {
                var next = grid.AdjacentCells(position, compare => compare.Target.Point != previous && compare.Target.Value != '#').First();
                previous = position.Point;
                position = next;
                path.Add(position.Point);
            }

            return path;

        }
    }
}
