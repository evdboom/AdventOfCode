using AdventOfCode2021.Extensions;
using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day11 : Day
    {
        public Day11(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 11;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid();

            long flashes = 0;
            for (int i = 0; i < 100; i ++)
            {
                flashes += ProcessDay(grid);
            }

            return flashes;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid();

            var size = grid.GetLength(0) * grid.GetLength(1);
            long flashes = 0;
            var steps = 0;
            while (flashes < size)
            {
                steps++;
                flashes = ProcessDay(grid);
            }

            return steps;
        }

        private long ProcessDay(int[,] grid)
        {            
            long flashes = 0;
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(1); i++)
                {                    
                    grid[i, j]++;
                }                
            }

            var any = true;
            while (any)
            {
                any = false;
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    for (int i = 0; i < grid.GetLength(1); i++)
                    {
                        if (grid[i, j] > 9)
                        {
                            any = true;
                            flashes++;
                            grid[i, j] = 0;
                            foreach (var point in grid.Adjacent(i, j, true))
                            {
                                if (grid[point.X, point.Y] != 0)
                                {
                                    grid[point.X, point.Y]++;
                                }
                            }
                        }
                    }
                }
            }

            return flashes;
        }      
    }
}
