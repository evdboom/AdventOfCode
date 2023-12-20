
namespace AdventOfCode2023.Days.Day20Group
{
    public class BroadcastModule : PulseModule
    {
        public override IEnumerable<(string Target, bool HighPulse, string Source)> ProcessPulse(string _1, bool _2)
        {
            foreach(var target in Targets)
            {
                LowPulsesSent++;
                yield return (target, false, Name);
            }
        }
    }
}
