using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days;

var importer = new FileImporter();
var writer = new ScreenWriter();
var days = new Dictionary<int, IDay>()
{
    { 1, new Day01(importer) },
    { 2, new Day02(importer) },
    { 3, new Day03(importer) },

};
var runner = new DayRunner(days, writer);
Console.WriteLine("Hello, Santa!");
await runner.Run();

Console.WriteLine("Press any key to close");
Console.ReadKey();