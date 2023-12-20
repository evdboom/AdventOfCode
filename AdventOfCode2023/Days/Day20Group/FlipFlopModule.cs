
using System.Formats.Tar;

namespace AdventOfCode2023.Days.Day20Group
{
    public class FlipFlopModule : PulseModule
    {        
        public bool On { get; set; }
        public override IEnumerable<(string Target, bool HighPulse, string Source)> ProcessPulse(string _, bool highPulse)
        {
            if (highPulse)
            {
                yield break;
            }
            else
            {
                On = !On;
            }

            foreach (var target in Targets)
            {
                if (On)
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
