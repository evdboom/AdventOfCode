using AdventOfCode2021.Services;
using System.Text;

namespace AdventOfCode2021.Days
{
    public class Day20 : Day
    {
        private const char LightPixel = '#';
        private const char DarkPixel = '.';

        private readonly IScreenWriter _writer;

        public Day20(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;            
        }

        public override int DayNumber => 20;

        protected override long ProcessPartOne(string[] input)
        {
            _writer.Disable();
            return Process(input, 2);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            _writer.Disable();
            return Process(input, 50);
        }

        private long Process(string[] input, int runs)
        {
            var algorithm = input[0];
            var grid = GetGrid(input);
            var step = 0;
            DrawGrid(grid);
            while (step < runs)
            {                
                grid = PrepareGrid(grid, step == 0);
                grid = ProcessGrid(grid, algorithm);
                DrawGrid(grid);
                step++;
            }

            return CountPixels(grid);
        }

        private void DrawGrid(int[,] grid)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    _writer.Write(grid[i, j] == 1 ? LightPixel : DarkPixel);
                }
                _writer.NewLine();
            }
        }

        private long CountPixels(int[,] grid)
        {
            return grid.Cast<int>().Sum();
        }

        private int[,] ProcessGrid(int[,] grid, string algorithm)
        {
            var unknowns = grid[0, 0];
            var result = new int[grid.GetLength(0), grid.GetLength(1)];
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    result[i, j] = GetPixel(grid, i, j, unknowns, algorithm);
                }
            }

            return result;
        }

        private int GetPixel(int[,] grid, int column, int row, int unknowns, string algorithm)
        {
            StringBuilder builder = new StringBuilder();
            for (int j = row - 1; j <= row + 1; j++)
            {
                for (int i = column - 1; i <= column + 1; i++)
                {
                    var value = i < 0 || i == grid.GetLength(0) || j < 0 || j == grid.GetLength(1)
                        ? unknowns
                        : grid[i, j];
                    builder.Append(value);
                }
            }

            var result = Convert.ToInt32(builder.ToString(), 2);
            return algorithm[result] == LightPixel
                ? 1
                : 0;
        }

        private int[,] PrepareGrid(int[,] grid, bool first = false)
        {
            var factor = first ? 2 : 1;
            var result = new int[grid.GetLength(0) + 2 * factor, grid.GetLength(1) + 2 * factor];
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    result[i + factor, j + factor] = grid[i, j];
                }
            }

            var value = grid[0, 0];
            if (!first && value == 1)
            {               
                for (int j = 0; j < factor; j++)
                {
                    for (int i = 0; i < result.GetLength(0); i++)
                    {
                        result[i, j] = value;
                        result[i, result.GetLength(1) - 1 - j] = value;
                    }
                }
                for (int i = 0; i < factor; i++)
                {
                    for (int j = 0; j < result.GetLength(1); j++)
                    {
                        result[i, j] = value;
                        result[result.GetLength(0) - 1 - i, j] = value;
                    }
                }
            }

            return result;
        }

        private int[,] GetGrid(string[] input)
        {
            var width = input[2].Length;
            var height = input.Length - 2;
            var result = new int[width, height];
            
            for (int j = 0; j < height; j++)
            {                
                for (int i = 0; i < width; i++)
                {
                    result[i, j] = input[j + 2][i] == LightPixel ? 1 : 0;                        
                }
            }

            return result;
        }
    }
}
