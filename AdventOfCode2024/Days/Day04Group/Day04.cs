using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
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
            
            return grid
                .Where(cell => cell.Value == _mappings['X'])
                .Sum(cell => GetWords(grid, cell.Point)
                    .Where(word => word)
                    .Count());
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid(c => _mappings.TryGetValue(c, out var value) ? value : -1);            
            
            return grid
                .Where(cell => cell.Value == _mappings['A'] && GetX(grid, cell.Point))
                .Count();           
        }

        private bool GetX(Grid<int> grid, Point point)
        {
            var adjecents = grid
                .AdjecentWithDirection(point, (compare) => compare.Target == _mappings['M'] || compare.Target == _mappings['S'], Directions.Diagonal)
                .ToList();

            return 
                adjecents
                    .Where(adjecent => (Directions.Slash & adjecent.Direction) > 0)
                    .Sum(adjecent => adjecent.Value) == _mappings['M'] + _mappings['S'] &&
                adjecents
                    .Where(adjecent => (Directions.Backslash & adjecent.Direction) > 0)
                    .Sum(adjecent => adjecent.Value) == _mappings['M'] + _mappings['S'];
        }

        private IEnumerable<bool> GetWords(Grid<int> grid, Point point)
        {
            foreach(var adjecent in grid.AdjacentWithDirection(point, (compare) => compare.Target == 1, true))
            {
                yield return NextIsValid(grid, adjecent.Point, adjecent.Direction);
            }                       
        }

        private bool NextIsValid(Grid<int> grid, Point point, Directions direction)
        {
            if (grid[point] == EndOfWord)
            {
                return true;
            }

            if (grid.TryGetPointInDirection(point, direction, (compare) => compare.Target == compare.Origin + 1, out var nextPoint))
            {
                return NextIsValid(grid, nextPoint.Value, direction);
            }

            return false;
        }
    }
}
