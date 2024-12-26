using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day25(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 25;

        protected override long ProcessPartOne(string[] input)
        {
            var locksAndKeys = GetLocksAndKeys(input);
            var locks = locksAndKeys.Where(l => l.IsLock).Select(l => l.Grid).ToList();
            var keys = locksAndKeys.Where(l => !l.IsLock).Select(l => l.Grid).ToList();

            var result = 0L;
            foreach (var lck in locks)
            {
                result += keys.Aggregate(0L, (acc, key) =>
                {
                    if (key.Zip(lck).All(lk => !lk.First.Value || !lk.Second.Value))
                    {
                        acc++;
                    }
                    return acc;
                });
            }

            return result;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return -1;
        }

        private IEnumerable<(Grid<bool> Grid, bool IsLock)> GetLocksAndKeys(string[] input)
        {
            var parts = string.Join('\n', input).Split("\n\n");
            foreach (var part in parts)
            {
                var lines = part.Split('\n');
                var grid = lines.ToGrid(l => l == '#');

                yield return (grid, grid[0,0]);
            }
        }
    }
}
