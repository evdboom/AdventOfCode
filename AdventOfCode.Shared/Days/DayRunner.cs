using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Services;

namespace AdventOfCode.Shared.Days
{
    public class DayRunner
    {
        private const string All = "All";

        private readonly IDictionary<int, IDay> _days;
        private readonly IScreenWriter _writer;

        public DayRunner(IDictionary<int, IDay> days, IScreenWriter writer)
        {
            _days = days;
            _writer = writer;
        }
   
        public async Task Run()
        {            
            bool running = true;
            while (running)
            {
                Console.WriteLine($"Which day would you like to process? ({_days.Keys.Min()}-{_days.Keys.Max()} or '{All}' for a complete run)");
                var selectedDay = 0;
                while (selectedDay == 0)
                {
                    var input = Console.ReadLine();
                    if (input?.ToLower() == All.ToLower())
                    {
                        selectedDay = -1;
                    }
                    else if (!int.TryParse(input, out int day) || day < _days.Min(d => d.Key) || day > _days.Max(d => d.Key))
                    {
                        Console.WriteLine($"Enter a valid day (from {_days.Min(d => d.Key)} to {_days.Max(d => d.Key)})");
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
                    await ProcessDayAsync(_days[selectedDay], 1);
                }

                Console.Write("Would you like to process another day? (y/N): ");
                var key = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Y)
                {
                    running = false;
                }
            }
        }

        async Task ProcessAllAsync(int runs)
        {
            var memory = GC.GetTotalMemory(true);
            foreach (var day in _days)
            {
                await ProcessDayAsync(day.Value, runs);
            }
            Console.WriteLine();
            var newMemory = GC.GetTotalMemory(false) - memory;
            Console.WriteLine($"Memory increase (before GC): {newMemory / 1024} kB");
            newMemory = GC.GetTotalMemory(true) - memory;
            Console.WriteLine($"Memory increase (after GC): {newMemory / 1024} kB");
        }

        private async Task ProcessDayAsync(IDay day, int runs)
        {
            await ProcessPartAsync(day, Part.One, runs);
            await ProcessPartAsync(day, Part.Two, runs);
            Console.WriteLine();
        }

        private async Task ProcessPartAsync(IDay day, Part part, int runs)
        {
            try
            {
                long answered = 0;
                List<long> durations = new();
                string run = string.Empty;
                for (int i = 0; i < runs; i++)
                {
                    var (answer, duration) = await day.ProcessPartAsync(part);
                    answered = answer;
                    durations.Add(duration);
                    _writer.Disable();
                    run = $"Day {day.DayNumber}, part {part}: run {i + 1}/{runs} completed";
                    Console.Write(run);
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top);

                }
                Console.Write(new string(' ', run.Length));
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                _writer.Enable();
                Console.WriteLine($"The answer for day {day.DayNumber}, part {part} is: {answered}");

                var avg = Math.Round(durations.Average());
                var message = runs > 1
                    ? $"Processing took an average of {avg} ms over {runs} runs (min {durations.Min()} ms, max {durations.Max()} ms)"
                    : $"Processing took {durations[0]} ms";
                Console.WriteLine(message);
            }
            catch (NotImplementedException)
            {
                Console.WriteLine($"Day {day.DayNumber} part {part} has not yet been solved.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
