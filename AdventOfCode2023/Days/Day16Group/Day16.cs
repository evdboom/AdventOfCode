using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day16 : Day
    {
        public Day16(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 16;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid(c => c);

            return GetEnergizedTiles(-1, 0, 'R', grid);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid(c => c);
            return GetStartOptions(grid)
                .Select(option => GetEnergizedTiles(option.X, option.Y, option.Direction, grid))
                .Max();
        }

        private IEnumerable<(int X, int Y, char Direction)> GetStartOptions(Grid<char> grid)
        {
            foreach (var optionX in Enumerable.Range(0, grid.Width))
            {
                yield return (optionX, -1, 'D');
                yield return (optionX, grid.Height, 'U');
            }
            foreach (var optionY in Enumerable.Range(0, grid.Height))
            {
                yield return (-1, optionY, 'R');
                yield return (grid.Width, optionY, 'L');
            }
        }

        private long GetEnergizedTiles(int startX, int startY, char startDirection, Grid<char> grid)
        {
            var energized = new List<(int X, int Y, char Direction)>();

            List<(int X, int Y, char Direction)> steps = [(startX, startY, startDirection)];

            do
            {
                var newSteps = new List<(int X, int Y, char Direction)>();
                foreach (var step in steps)
                {
                    if (!energized.Contains(step))
                    {
                        energized.Add(step);
                        newSteps.AddRange(GetNextSteps(step, grid));
                    }
                }
                steps = newSteps;
            }
            while (steps.Any());

            return energized
                .GroupBy(item => new { item.X, item.Y })
                .Count() - 1;
        }

        private IEnumerable<(int X, int Y, char Direction)> GetNextSteps((int X, int Y, char Direction) location, Grid<char> grid)
        {
            switch (location.Direction)
            {
                case 'R':
                    if (location.X < grid.Width - 1)
                    {
                        var newLocation = grid[location.X + 1, location.Y];
                        switch (newLocation)
                        {
                            case '/':
                                yield return (location.X + 1, location.Y, 'U');
                                break;
                            case '\\':
                                yield return (location.X + 1, location.Y, 'D');
                                break;
                            case '|':
                                yield return (location.X + 1, location.Y, 'D');
                                yield return (location.X + 1, location.Y, 'U');
                                break;
                            default:
                                yield return (location.X + 1, location.Y, 'R');
                                break;
                        }
                    }
                    break;
                case 'L':
                    if (location.X > 0)
                    {
                        var newLocation = grid[location.X - 1, location.Y];
                        switch (newLocation)
                        {
                            case '/':
                                yield return (location.X - 1, location.Y, 'D');
                                break;
                            case '\\':
                                yield return (location.X - 1, location.Y, 'U');
                                break;
                            case '|':
                                yield return (location.X - 1, location.Y, 'D');
                                yield return (location.X - 1, location.Y, 'U');
                                break;
                            default:
                                yield return (location.X - 1, location.Y, 'L');
                                break;
                        }
                    }
                    break;
                case 'U':
                    if (location.Y > 0)
                    {
                        var newLocation = grid[location.X, location.Y - 1];
                        switch (newLocation)
                        {
                            case '/':
                                yield return (location.X, location.Y - 1, 'R');
                                break;
                            case '\\':
                                yield return (location.X, location.Y - 1, 'L');
                                break;
                            case '-':
                                yield return (location.X, location.Y - 1, 'L');
                                yield return (location.X, location.Y - 1, 'R');
                                break;
                            default:
                                yield return (location.X, location.Y - 1, 'U');
                                break;
                        }
                    }
                    break;
                case 'D':
                    if (location.Y < grid.Height - 1)
                    {
                        var newLocation = grid[location.X, location.Y + 1];
                        switch (newLocation)
                        {
                            case '/':
                                yield return (location.X, location.Y + 1, 'L');
                                break;
                            case '\\':
                                yield return (location.X, location.Y + 1, 'R');
                                break;
                            case '-':
                                yield return (location.X, location.Y + 1, 'L');
                                yield return (location.X, location.Y + 1, 'R');
                                break;
                            default:
                                yield return (location.X, location.Y + 1, 'D');
                                break;
                        }
                    }
                    break;
            }
        }
    }
}