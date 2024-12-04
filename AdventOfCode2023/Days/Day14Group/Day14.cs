using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day14 : Day
    {
        public Day14(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 14;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid(c =>
            {
                return c switch
                {
                    'O' => (bool?)true,
                    '#' => false,
                    _ => null
                };
            });

            MoveNorth(grid);
            return CalculateLoad(grid);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid(c =>
            {
                return c switch
                {
                    'O' => (bool?)true,
                    '#' => false,
                    _ => null
                };
            });

            var known = new Dictionary<long, (long Hash, int Index)>();
            var shifted = false;
            for (long i = 0; i < 1000000000; i++)
            {
                var hash = GetHash(grid);
                if (known.TryGetValue(hash, out var knownHash) && ! shifted)
                {
                    var validForCycle = known
                        .Where(h => h.Value.Index >= knownHash.Index)
                        .Count();

                    var remaining = (1000000000 - i) / validForCycle;
                    i += remaining * validForCycle;
                    shifted = true;

                }
                
                Cycle(grid);
                known[hash] = (GetHash(grid), known.Count);                                
            }
            return CalculateLoad(grid);
        }

        private long GetHash(Grid<bool?> grid)
        {
            var result = 0L;
            for (int j = 0; j < grid.Height; j++)
            {
                for (int i = 0; i < grid.Width; i++)
                {
                    if (grid[i, j] ?? false)
                    {
                        result += i * j;
                    }
                }
            }
            return result;
        }

        private void Cycle(Grid<bool?> grid)
        {
            MoveNorth(grid);
            MoveWest(grid);
            MoveSouth(grid);
            MoveEast(grid);
        }

        private void MoveNorth(Grid<bool?> grid)
        {
            for (int i = 0; i < grid.Width; i++)
            {
                var canMoveTo = 0;
                for (int j = 0; j < grid.Height; j++)
                {
                    var value = grid[i, j];
                    if (value.HasValue && !value.Value)
                    {
                        canMoveTo = j + 1;
                    }
                    else if (value.HasValue)
                    {
                        grid[i, j] = null;
                        grid[i, canMoveTo] = true;
                        canMoveTo++;
                    }
                }
            }
        }

        private void MoveWest(Grid<bool?> grid)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                var canMoveTo = 0;
                for (int i = 0; i < grid.Width; i++)
                {
                    var value = grid[i, j];
                    if (value.HasValue && !value.Value)
                    {
                        canMoveTo = i + 1;
                    }
                    else if (value.HasValue)
                    {
                        grid[i, j] = null;
                        grid[canMoveTo, j] = true;
                        canMoveTo++;
                    }
                }
            }
        }

        private void MoveSouth(Grid<bool?> grid)
        {
            for (int i = 0; i < grid.Width; i++)
            {
                var canMoveTo = grid.Height - 1;
                for (int j = grid.Height -1 ; j >= 0; j--)
                {
                    var value = grid[i, j];
                    if (value.HasValue && !value.Value)
                    {
                        canMoveTo = j - 1;
                    }
                    else if (value.HasValue)
                    {
                        grid[i, j] = null;
                        grid[i, canMoveTo] = true;
                        canMoveTo--;
                    }
                }
            }
        }

        private void MoveEast(Grid<bool?> grid)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                var canMoveTo = grid.Width - 1;
                for (int i = grid.Width - 1; i >= 0; i--)
                {
                    var value = grid[i, j];
                    if (value.HasValue && !value.Value)
                    {
                        canMoveTo = i - 1;
                    }
                    else if (value.HasValue)
                    {
                        grid[i, j] = null;
                        grid[canMoveTo, j] = true;
                        canMoveTo--;
                    }
                }
            }
        }

        private long CalculateLoad(Grid<bool?> grid)
        {
            var result = 0L;
            var distance = grid.Width;
            for (int j = 0; j < grid.Height; j++)
            {
                for (int i = 0; i < grid.Width; i++)
                {
                    if (grid[i, j] ?? false)
                    {
                        result += distance - j;
                    }
                }
            }

            return result;
        }
    }
}