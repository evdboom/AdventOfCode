using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using AdventOfCode2025.Days.Day04Group;

namespace AdventOfCode2025.Days;

public class Day04(IFileImporter fileImporter) : Day(fileImporter)
{
    public override int DayNumber => 4;

    protected override long ProcessPartOne(string[] input)
    {
        return GetPaperRolls(input).Count(roll => roll.CanRemove);
    }

    protected override long ProcessPartTwo(string[] input)
    {
        var rolls = GetPaperRolls(input);
        var removablePaperRolls = rolls.Where(roll => roll.CanRemove).ToList();

        foreach (var roll in removablePaperRolls)
        {
            roll.RemoveIfAble();
        }

        return rolls.Count(roll => roll.Removed);
    }

    private static List<PaperRoll> GetPaperRolls(string[] input)
    {
        var grid = input.ToGrid(c => c == '@' ? new PaperRoll() : null);
        return grid.Where(cell => cell.Value is not null)
            .Select(cell =>
            {
                cell.Value!.AdjacentRolls.AddRange(
                    grid.AdjacentCells(cell, compare => compare.Target.Value is not null, true)
                        .Select(t => t.Value!)
                );

                return cell.Value;
            })
            .ToList();
    }
}
