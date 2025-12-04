using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days;

public class Day04(IFileImporter fileImporter) : Day(fileImporter)
{
    public override int DayNumber => 4;

    protected override long ProcessPartOne(string[] input)
    {
        var grid = input.ToGrid();
        return grid.Count(cell =>
            cell.Value == '@'
            && grid.Adjacent(cell.Point, cell => cell.Target == '@', allowDiagonal: true).Count()
                < 4
        );
    }

    protected override long ProcessPartTwo(string[] input)
    {
        var grid = input.ToGrid();
        var removed = true;
        var count = 0;
        while (removed)
        {
            removed = false;
            var removableCells = grid.Where(cell =>
                cell.Value == '@'
                && grid.Adjacent(cell.Point, cell => cell.Target == '@', allowDiagonal: true)
                    .Count() < 4
            );

            foreach (var cell in removableCells)
            {
                count++;
                removed = true;
                grid[cell.Point] = '.';
            }
        }

        return count;
    }
}
