using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day20Group;
using System.Reflection;

namespace AdventOfCode2023.Days
{
    public class Day20 : Day
    {
        public Day20(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 20;

        protected override long ProcessPartOne(string[] input)
        {
            var modules = GetModules(input);
            return ProcessPushes(1000L, modules);
        }

        private long _buttonPushes;
        private Dictionary<string, long> _targets = [];

        protected override long ProcessPartTwo(string[] input)
        {
            var modules = GetModules(input);

            // no rx in test data
            if (modules.Values
                .FirstOrDefault(module => module.Targets.Contains("rx")) is not ConjunctionModule target)
            {
                return -1;
            }

            target.HighPulseReceived += HighPulseReceived;

            while (_targets.Count < target.InputModules.Count)
            {
                _buttonPushes++;
                PushButton(modules);
            }

            return _targets
                .Aggregate(1L, (value, item) => value * item.Value);
        }

        private void HighPulseReceived(object? _, string source)
        {
            if (!_targets.ContainsKey(source))
            {
                _targets[source] = _buttonPushes;
            }            
        }

        private long ProcessPushes(long pushCount, Dictionary<string, PulseModule> modules)
        {
            for (long i = 0; i < pushCount; i++)
            {
                PushButton(modules);
            }

            var (low, high) = modules.Aggregate((Low: 0L, High: 0L), (value, module) =>
            {
                value.Low += module.Value.LowPulsesSent;
                value.High += module.Value.HighPulsesSent;
                return value;
            });
            return low * high;
        }

        private Dictionary<string, PulseModule> GetModules(string[] input)
        {
            var modules = input
                .Select(ToModule)
                .ToDictionary(module => module.Name, module => module);

            foreach (ConjunctionModule conjunction in modules.Values.Where(module => module is ConjunctionModule))
            {
                conjunction.InputModules = modules
                    .Where(module => module.Value.Targets.Contains(conjunction.Name))
                    .ToDictionary(module => module.Key, _ => false);
            }

            modules["button"] = new BroadcastModule
            {
                Name = "button",
                Targets = ["broadcaster"]
            };

            return modules;
        }

        private void PushButton(Dictionary<string, PulseModule> modules)
        {
            List<(string Module, bool HighPulse, string Source)> toProcess = [("button", false, "Santa")];

            while(toProcess.Any())
            {
                toProcess = toProcess
                    .Where(module => modules.ContainsKey(module.Module))
                    .SelectMany(module => modules[module.Module].ProcessPulse(module.Source, module.HighPulse))
                    .ToList();
            }           
        }

        private PulseModule ToModule(string line)
        {
            var parts = line.Split(" -> ");
            var targets = parts[1].Split(", ");
            if (parts[0] == "broadcaster")
            {
                return new BroadcastModule
                {
                    Name = parts[0],
                    Targets = targets
                };
            }
            else if (parts[0][0] == '%')
            {
                return new FlipFlopModule
                {
                    Name = parts[0][1..],
                    Targets = targets
                };
            }
            else
            {
                return new ConjunctionModule
                {
                    Name = parts[0][1..],
                    Targets = targets
                };
            }
        }
    }
}