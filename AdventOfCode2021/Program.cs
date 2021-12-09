using AdventOfCode2021.Days;
using AdventOfCode2021.Enums;
using AdventOfCode2021.Importers;

const string All = "All";

var importer = new FileImporter();
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
    { 13, new Day13(importer) },
    { 14, new Day14(importer) },
    { 15, new Day15(importer) },
    { 16, new Day16(importer) },
    { 17, new Day17(importer) },
    { 18, new Day18(importer) },
    { 19, new Day19(importer) },
    { 20, new Day20(importer) },
    { 21, new Day21(importer) },
    { 22, new Day22(importer) },
    { 23, new Day23(importer) },
    { 24, new Day24(importer) },
    { 25, new Day25(importer) },
};

Console.WriteLine("Hello, Santa!");
Console.WriteLine($"Which day would you like to process? ({days.Keys.Min()}-{days.Keys.Max()} or 'All' for a complete run)");
var selectedDay = 0;
while (selectedDay == 0)
{
    var input = Console.ReadLine();
    if (input == All)
    {        
        selectedDay = -1;        
    }
    else if (!int.TryParse(input, out int day) || day < days.Min(d => d.Key) || day > days.Max(d => d.Key))
    {
        Console.WriteLine($"Enter a valid day (from {days.Min(d => d.Key)} to { days.Max(d => d.Key)})");
    }
    else
    {
        selectedDay = day;
    }
}

if (selectedDay == -1)
{
    Console.WriteLine("How many runs would you like?");
    var runs = 0;
    while (runs == 0)
    {
        var runInput = Console.ReadLine();
        if (int.TryParse(runInput, out int parsedRuns) && parsedRuns > 0)
        {
            runs = parsedRuns;
        }
        else
        {
            Console.WriteLine("Please enter a positive integer");
        }
    }
    await ProcessAllAsync(runs);
}
else
{
    await ProcessDayAsync(selectedDay, Part.One);
    await ProcessDayAsync(selectedDay, Part.Two);
}

Console.WriteLine("Press any key to close");
Console.ReadKey();

async Task ProcessAllAsync(int runs)
{
    var memory = GC.GetTotalMemory(true);
    foreach (var day in days)
    {
        Console.WriteLine($"Day {day.Key}");
        Console.WriteLine($"Part one:");
        try
        {
            await ProcessRunAsync(day.Value, Part.One, runs);
        }
        catch (NotImplementedException)
        {
            Console.WriteLine($"Day {day.Key}, part {Part.One} has not yet been solved.");
        }
        Console.WriteLine("");
        Console.WriteLine($"Part two:");
        try
        { 
            await ProcessRunAsync(day.Value, Part.Two, runs);
        }
        catch (NotImplementedException)
        {
            Console.WriteLine($"Day {day.Key}, part {Part.Two} has not yet been solved.");
        }
        Console.WriteLine("");
    }
    var newMemory = GC.GetTotalMemory(false) - memory;
    Console.WriteLine($"Memory increase (before GC): {newMemory / 1024} kB");
    newMemory = GC.GetTotalMemory(true) - memory;
    Console.WriteLine($"Memory increase (after GC): {newMemory / 1024} kB");
}

async Task ProcessRunAsync(IDay day, Part part, int runs)
{
    long answered = 0;
    List<long> durations = new();
    for (int i = 0; i < runs; i++)
    {
        var (answer, duration) = await day.ProcessPartAsync(part);
        answered = answer;
        durations.Add(duration);
    }
    Console.WriteLine($"The answer is: {answered}");
    var avg = Math.Round(durations.Average());
    Console.WriteLine($"Processing took an average of {avg} ms over {runs} runs (min {durations.Min()} ms, max {durations.Max()} ms)");
}

async Task ProcessDayAsync(int day, Part part)
{
    Console.WriteLine("");
    try
    {
        var (answer, duration) = await days[day].ProcessPartAsync(part);

        Console.WriteLine($"The answer for part {part} is: {answer}");
        Console.WriteLine($"Processing took {duration} ms");
    }
    catch (NotImplementedException)
    {
        Console.WriteLine($"Day {day} part {part} has not yet been solved.");
    }
    Console.WriteLine("");
}