using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System.Reflection;

var importer = new FileImporter();
var writer = new ScreenWriter();
var days = Assembly
    .GetEntryAssembly()?
    .GetTypes()
    .Where(p => typeof(IDay).IsAssignableFrom(p) && string.Equals(p.Namespace, "AdventOfCode2024.Days"))
    .Select(CreateDay)
    .Where(p => p is not null)
    .ToDictionary(day => day!.DayNumber, day => day);

var runner = new DayRunner(days!, writer);
Console.WriteLine("Hello, Santa!");
await runner.Run();

Console.WriteLine("Press any key to close");
Console.ReadKey();

IDay? CreateDay(Type p)
{
    try
    {
        return Activator.CreateInstance(p, importer) as IDay;
    }
    catch
    {
        return Activator.CreateInstance(p, importer, writer) as IDay;
    }
}