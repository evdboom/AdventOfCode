using AdventOfCode2021.Extensions;
using AdventOfCode2021.Services;
using System.Drawing;

namespace AdventOfCode2021.Days
{
    public class Day15 : Day
    {

        public Day15(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 15;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid();
            var gridSum = new int[grid.GetLength(0), grid.GetLength(1)];

            for(int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i ++)
                {
                    gridSum[i, j] = int.MaxValue;
                }
            }

            Point current = new Point(0, 0);
            Point wanted = new Point(grid.GetLength(0) - 1, grid.GetLength(1) - 1);

            foreach (var p in grid.Adjacent(current.X, current.Y).Where(p => ValidDirection(current, p)))
            {
                ProjectPoint(0, new Point(p.X, p.Y), wanted, grid, gridSum);
            }

            return gridSum[wanted.X, wanted.Y];
        }

        private void ProjectPoint(int currentRisk, Point point, Point wanted, int[,] grid, int[,] gridSum)
        {
            currentRisk += grid[point.X, point.Y];

            if (currentRisk > gridSum[point.X, point.Y])
            {
                return;
            }
            else
            {
                gridSum[point.X, point.Y] = currentRisk;
            }

            foreach (var p in grid.Adjacent(point.X, point.Y).Where(p => ValidDirection(point, p)))
            {
                ProjectPoint(currentRisk, new Point(p.X, p.Y), wanted, grid, gridSum);
            }
        }

        private bool ValidDirection(Point current, (int X, int Y) p)
        {
            return
                p.X > current.X ||
                p.Y > current.Y;
        }


        protected override long ProcessPartTwo(string[] input)
        {
            throw new NotImplementedException();
        }
    }
}
