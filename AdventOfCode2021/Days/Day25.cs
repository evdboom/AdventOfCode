using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day25 : Day
    {
        private const char Empty = '.';
        private const char East = '>';
        private const char South = 'v';
     
        private readonly IScreenWriter _writer;

        public Day25(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }

        public override int DayNumber => 25;

        protected override long ProcessPartOne(string[] input)
        {
            _writer.Disable();
            var grid = GetGrid(input);

            PrintGrid(grid);
            long steps = 0;
            bool hasMoved;
            do
            {
                steps++;
                var eastMoved = ProcessStep(grid, East);
                var southMoved = ProcessStep(grid, South);
                hasMoved = eastMoved || southMoved;               
                PrintGrid(grid);
            }
            while (hasMoved);

            return steps;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            _writer.Enable();
            _writer.NewLine();
            _writer.Write("All done!, ");
            _writer.Write("Merry", ConsoleColor.Red);
            _writer.WriteLine(" Chirstmas", ConsoleColor.Green);
            _writer.NewLine();
            return 0;
        }

        private bool ProcessStep(char[,] grid, char direction)
        {
            var moves = new Dictionary<(int x, int y), (int x, int y)>();
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (grid[i, j] != direction)
                    {
                        continue;
                    }

                    var target = GetTarget(i, j, grid);
                    if (grid[target.x, target.y] == Empty)
                    {
                        moves[(i, j)] = target;
                    }
                }
            }
                    
            foreach (var move in moves)
            {
                grid[move.Key.x, move.Key.y] = Empty;
                grid[move.Value.x, move.Value.y] = direction;
            }

            return moves.Any();
        }

        private (int x, int y) GetTarget(int x, int y, char[,] grid)
        {
            return grid[x,y] switch
            {
                East => (x == grid.GetLength(0) - 1 ? 0 : x + 1, y),
                South => (x, y == grid.GetLength(1) - 1 ? 0 : y + 1),
                _ => default
            };
        }

        private void PrintGrid(char[,] grid)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    _writer.Write(grid[i, j]);

                }
                _writer.NewLine();
            }
            _writer.NewLine();
        }

        private char[,] GetGrid(string[] input)
        {
            var result = new char[input[0].Length, input.Length];

            for (int j = 0; j < result.GetLength(1); j++)
            {
                for (int i = 0; i < result.GetLength(0); i++)
                {
                    result[i, j] = input[j][i];
                }
            }

            return result;
        }
    }
}
