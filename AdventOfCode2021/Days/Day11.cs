using AdventOfCode2021.Importers;

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
            var grid = GetGrid(input);

            long flashes = 0;
            for (int i = 0; i < 100; i ++)
            {
                flashes += ProcessDay(grid);
            }

            return flashes;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = GetGrid(input);

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
                            foreach ((int x, int y) in Adjacent(i, j, grid))
                            {
                                if (grid[x, y] != 0)
                                {
                                    grid[x, y]++;
                                }
                            }
                        }
                    }
                }
            }

            return flashes;
        }

        private IEnumerable<(int X, int Y)> Adjacent(int x, int y, int[,] grid)
        {
            if (y > 0)
            {
                yield return (x, y - 1);
            }
            if (y < grid.GetLength(1) - 1)
            {
                yield return (x, y + 1);
            }
            if (x > 0)
            {
                yield return (x - 1, y);
            }
            if (x < grid.GetLength(0) - 1)
            {
                yield return (x + 1, y);
            }
            if (x > 0 && y > 0)
            {
                yield return (x - 1, y - 1);
            }
            if (x > 0 && y < grid.GetLength(1) - 1)
            {
                yield return (x - 1, y + 1);
            }
            if (x < grid.GetLength(0) - 1 && y > 0)
            {
                yield return (x + 1, y - 1);
            }
            if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1)
            {
                yield return (x + 1, y + 1);
            }
        }


        private int[,] GetGrid(string[] input)
        {
            var grid = new int[input[0].Length, input.Length];

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    grid[i, j] = int.Parse($"{input[j][i]}");
                }
            }

            return grid;
        }
    }
}
