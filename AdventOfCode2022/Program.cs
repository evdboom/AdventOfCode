using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;


var importer = new FileImporter();
var writer = new ScreenWriter();
var days = new Dictionary<int, IDay>();
var runner = new DayRunner(days, writer);
Console.WriteLine("Hello, Santa!");
await runner.Run();

Console.WriteLine("Press any key to close");
Console.ReadKey();