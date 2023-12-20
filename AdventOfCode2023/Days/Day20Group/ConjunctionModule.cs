
namespace AdventOfCode2023.Days.Day20Group
{
    public class ConjunctionModule : PulseModule
    {
        public event EventHandler<string>? HighPulseReceived;
        public Dictionary<string, bool> InputModules { get; set; } = [];

        public override IEnumerable<(string Target, bool HighPulse, string Source)> ProcessPulse(string source, bool highPulse)
        {
            InputModules[source] = highPulse;

            if (Targets.Contains("rx") && highPulse)
            {
                HighPulseReceived?.Invoke(this, source);
            }

            var high = InputModules.Values.Any(module => !module);

            foreach(var target in Targets)
            {
                if (high)
                {
                    HighPulsesSent++;
                    yield return (target, true, Name);
                }
                else
                {
                    LowPulsesSent++;
                    yield return (target, false, Name);
                }
            }
        }
    }
}
