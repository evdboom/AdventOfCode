using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2021.Days;

var importer = new FileImporter();
var writer = new ScreenWriter();
var days = new Dictionary<int, IDay>
{
    { 1, new Day01(importer) },
    { 2, new Day02(importer) },
    { 3, new Day03(importer) },
    { 4, new Day04(importer) },
    { 5, new Day05(importer) },
    { 6, new Day06(importer) },
    { 7, new Day07(importer) },
    { 8, new Day08(importer) },
    { 9, new Day09(importer) },
    { 10, new Day10(importer) },
    { 11, new Day11(importer) },
    { 12, new Day12(importer) },
    { 13, new Day13(importer, writer) },
    { 14, new Day14(importer) },
    { 15, new Day15(importer) },
    { 16, new Day16(importer) },
    { 17, new Day17(importer) },
    { 18, new Day18(importer, writer) },
    { 19, new Day19(importer, writer) },
    { 20, new Day20(importer, writer) },
    { 21, new Day21(importer) },
    { 22, new Day22(importer) },
    { 23, new Day23(importer, writer) },
    { 24, new Day24(importer) },
    { 25, new Day25(importer, writer) },
};
var runner = new DayRunner(days, writer);
Console.WriteLine("Hello, Santa!");
await runner.Run();

Console.WriteLine("Press any key to close");
Console.ReadKey();