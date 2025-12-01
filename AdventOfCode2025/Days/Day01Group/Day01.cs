using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days;

public class Day01(IFileImporter fileImporter) : Day(fileImporter)
{
    private const int MaxDial = 100;
    public override int DayNumber => 1;

    protected override long ProcessPartOne(string[] input)
    {
        return ParseInput(input)
            .Aggregate(
                (Dial: 50, TimesZero: 0L),
                (acc, ticks) =>
                {
                    var newDial = acc.Dial + ticks % MaxDial;

                    if (newDial < 0)
                    {
                        newDial += MaxDial;
                    }
                    else if (newDial == 0)
                    {
                        acc.TimesZero++;
                    }

                    return (Dial: newDial, acc.TimesZero);
                }
            )
            .TimesZero;
    }

    protected override long ProcessPartTwo(string[] input)
    {
        return ParseInput(input)
            .Aggregate(
                (Dial: 50, TimesZero: 0L),
                (acc, ticks) =>
                {
                    var newDial = acc.Dial + ticks;
                    var padded = newDial.ToString().PadLeft(4);
                    Console.Write($"\t => {padded}");

                    var timesPastZero = Math.Abs(newDial / MaxDial);
                    var spare = Math.Abs(newDial % MaxDial);

                    if (spare == 0 && timesPastZero > 0)
                    {
                        // Ended on zero, count a crossing less
                        timesPastZero--;
                    }

                    if (Math.Sign(newDial) < 0 && Math.Sign(acc.Dial) != 0)
                    {
                        // Moving negative past zero
                        timesPastZero++;
                    }

                    acc.TimesZero += timesPastZero;

                    newDial %= MaxDial;
                    if (newDial < 0)
                    {
                        newDial += MaxDial;
                    }
                    else if (newDial == 0)
                    {
                        acc.TimesZero++;
                    }

                    return (Dial: newDial, acc.TimesZero);
                }
            )
            .TimesZero;
    }

    private static List<int> ParseInput(string[] input)
    {
        return input.Select(i => i[0] == 'L' ? -int.Parse(i[1..]) : int.Parse(i[1..])).ToList();
    }
}
