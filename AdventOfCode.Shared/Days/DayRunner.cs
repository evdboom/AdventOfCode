using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Services;

namespace AdventOfCode.Shared.Days
{
    public class DayRunner : IDayRunner
    {
        private const string All = "All";
        private const string Loop = "Loop";

        private readonly IDictionary<int, IDay> _days;
        private readonly IScreenWriter _writer;

        public DayRunner(IEnumerable<IDay> days, IScreenWriter writer)
        {
            _days = days
                .ToDictionary(day => day.DayNumber, day => day);
            _writer = writer;
        }
   
        public async Task Run()
        {            
            bool running = true;
            while (running)
            {
                _writer.WriteLine($"Which day would you like to process? ({_days.Keys.Min()}-{_days.Keys.Max()} use '{All}' for a complete run, or '{Loop}' to loop a single day)");
                var selectedDay = 0;
                var loopDay = 0;
                while (selectedDay == 0)
                {
                    var input = _writer.ReadLine();
                    if (string.Equals(input, All, StringComparison.OrdinalIgnoreCase))
                    {
                        selectedDay = -1;
                    }
                    else if (string.Equals(input, Loop, StringComparison.OrdinalIgnoreCase))
                    {
                        selectedDay = -1;
                        _writer.WriteLine($"Enter a day (from {_days.Min(d => d.Key)} to {_days.Max(d => d.Key)}) to loop");
                        while (loopDay == 0)
                        {
                            input = _writer.ReadLine();
                            if (int.TryParse(input, out int day) && day >= _days.Min(d => d.Key) && day <= _days.Max(d => d.Key))
                            {
                                loopDay = day;
                            }
                            else
                            {
                                _writer.WriteLine($"Enter a valid day (from {_days.Min(d => d.Key)} to {_days.Max(d => d.Key)})");
                            }
                        }
                        
                    }
                    else if (!int.TryParse(input, out int day) || day < _days.Min(d => d.Key) || day > _days.Max(d => d.Key))
                    {
                        _writer.WriteLine($"Enter a valid day (from {_days.Min(d => d.Key)} to {_days.Max(d => d.Key)})");
                    }
                    else
                    {
                        selectedDay = day;
                    }
                }

                if (selectedDay == -1)
                {
                    _writer.WriteLine("How many runs would you like?");
                    var runs = 0;
                    while (runs == 0)
                    {
                        var runInput = _writer.ReadLine();
                        if (int.TryParse(runInput, out int parsedRuns) && parsedRuns > 0)
                        {
                            runs = parsedRuns;
                        }
                        else
                        {
                            _writer.WriteLine("Please enter a positive integer");
                        }
                    }
                    await ProcessLoopAsync(runs, loopDay);
                }
                else
                {
                    await ProcessDayAsync(_days[selectedDay], 1);
                }

                _writer.Write("Would you like to process another day? (y/N): ");
                var key = _writer.ReadKey();
                _writer.NewLine();
                _writer.NewLine();
                if (key != ConsoleKey.Y)
                {
                    running = false;
                }
            }
        }

        async Task ProcessLoopAsync(int runs, int singleDay = 0)
        {
            var memory = GC.GetTotalMemory(true);
            var days = singleDay == 0
                ? _days.Values
                : [_days[singleDay]];
            foreach (var day in days)
            {
                await ProcessDayAsync(day, runs);
            }
            _writer.NewLine();
            var newMemory = GC.GetTotalMemory(false) - memory;
            _writer.WriteLine($"Memory increase (before GC): {newMemory / 1024} kB");
            newMemory = GC.GetTotalMemory(true) - memory;
            _writer.WriteLine($"Memory increase (after GC): {newMemory / 1024} kB");
        }

        private async Task ProcessDayAsync(IDay day, int runs)
        {
            await ProcessPartAsync(day, Part.One, runs);
            await ProcessPartAsync(day, Part.Two, runs);
            _writer.NewLine();
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
                    run = $"Day {day.DayNumber}, part {part}: run {i + 1}/{runs} completed";
                    _writer.Enable();
                    _writer.Write(run);
                    _writer.SetStart();
                    _writer.Disable();
                }
                _writer.Write(new string(' ', run.Length));
                _writer.SetStart();
                _writer.Enable();
                _writer.WriteLine($"The answer for day {day.DayNumber}, part {part} is: {answered}");

                var avg = Math.Round(durations.Average());
                var message = runs > 1
                    ? $"Processing took an average of {avg} ms over {runs} runs (min {durations.Min()} ms, max {durations.Max()} ms)"
                    : $"Processing took {durations[0]} ms";
                _writer.WriteLine(message);
            }
            catch (NotImplementedException)
            {
                _writer.WriteLine($"Day {day.DayNumber} part {part} has not yet been solved.");
            }
            catch (FileNotFoundException ex)
            {
                _writer.WriteLine($"{ex.Message}");
            }
        }
    }
}
