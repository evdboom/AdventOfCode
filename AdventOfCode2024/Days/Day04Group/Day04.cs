using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day04 : Day
    {
        private const int EndOfWord = 3;
        private readonly Dictionary<char, int> _mappings = new()
        {
            { 'X', 0 },
            { 'M', 1 },
            { 'A', 2 },
            { 'S', 3 }
        };

        public Day04(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 4;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid(c => _mappings.TryGetValue(c, out var value) ? value : -1);

            var count = 0L;
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (grid[i, j] == _mappings['X'])
                    {
                        foreach(var word in GetWords(grid, i, j))
                        {
                            if (word)
                            {
                                count++;
                            }
                        }
                    }
                }
            }

            return count;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid(c => _mappings.TryGetValue(c, out var value) ? value : -1);

            var count = 0L;
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (grid[i, j] == _mappings['A'] && GetX(grid, i , j))
                    {                        
                            count++;                     
                    }
                }
            }

            return count;
        }

        private bool GetX(int[,] grid, int i, int j)
        {
            var diagonalDirections = new List<Direction>
            {
                Direction.UpLeft,
                Direction.UpRight,
                Direction.DownLeft,
                Direction.DownRight
            };

            var adjecents = grid
                .AdjecentWithDirection(i, j, (compare) => compare.Target == _mappings['M'] || compare.Target == _mappings['S'], true)
                .Where(adjecent => diagonalDirections.Contains(adjecent.Direction))
                .Select(adjecent => new 
                { 
                    Value = grid[adjecent.Point.X, adjecent.Point.Y],
                    adjecent.Direction 
                })
                .ToList();

            var upRight = adjecents.FirstOrDefault(adjecent => adjecent.Direction == Direction.UpRight);
            var downRight = adjecents.FirstOrDefault(adjecent => adjecent.Direction == Direction.DownRight);
            var upLeft = adjecents.FirstOrDefault(adjecent => adjecent.Direction == Direction.UpLeft);
            var downLeft = adjecents.FirstOrDefault(adjecent => adjecent.Direction == Direction.DownLeft);
            
            var slash = 
                upRight is not null && 
                downLeft is not null &&
                ((upRight.Value == _mappings['M'] && downLeft.Value == _mappings['S']) ||
                (upRight.Value == _mappings['S'] && downLeft.Value == _mappings['M']));
            var backSlash =
                upLeft is not null &&
                downRight is not null &&
                ((upLeft.Value == _mappings['M'] && downRight.Value == _mappings['S']) ||
                (upLeft.Value == _mappings['S'] && downRight.Value == _mappings['M']));
            return slash && backSlash;
        }

        private IEnumerable<bool> GetWords(int[,] grid, int i, int j)
        {
            foreach(var adjecent in grid.AdjecentWithDirection(i, j, (compare) => compare.Target == 1, true))
            {
                yield return NextIsValid(grid, adjecent.Point, adjecent.Direction);
            }                       
        }

        private bool NextIsValid(int[,] grid, Point point, Direction direction)
        {
            if (grid[point.X,point.Y] == EndOfWord)
            {
                return true;
            }

            if (grid.TryGetPointInDirection(point.X, point.Y, direction, (compare) => compare.Target == compare.Origin + 1, out var nextPoint))
            {
                return NextIsValid(grid, nextPoint.Value, direction);
            }

            return false;
        }
    }
}
