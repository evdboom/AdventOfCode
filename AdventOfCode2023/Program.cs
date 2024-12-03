using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days;

var runner = Runner.CreateRunner(typeof(Day01).Namespace);

Console.WriteLine("Hello, Santa!");
await runner.Run();
Console.WriteLine("Press any key to close");
Console.ReadKey();