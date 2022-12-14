using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day12Group;
using AdventOfCode2022.Days.Day13Group;

namespace AdventOfCode2022.Days
{
    public class Day13 : Day
    {
        public Day13(IFileImporter importer) : base(importer)
        {

        }

        public override int DayNumber => 13;
        protected override long ProcessPartOne(string[] input)
        {
            var packetPairs = ReadPackets(input);
            return packetPairs
                .Where(p => p.IsValid())
                .Select(p => p.Index)
                .Sum();
        }        

        protected override long ProcessPartTwo(string[] input)
        {
            var packets = input
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(ParsePacket)
                .ToList();

            var dividerTwo = new Packet();
            var innerTwo = new Packet { Parent = dividerTwo };
            dividerTwo.InnerPackets.Add(innerTwo);
            innerTwo.InnerPackets.Add(new Packet { Parent = innerTwo, Value = 2 });

            var dividerSix = new Packet();
            var innerSix = new Packet { Parent = dividerSix };
            dividerSix.InnerPackets.Add(innerSix);
            innerSix.InnerPackets.Add(new Packet { Parent = innerSix, Value = 6 });

            packets.Add(dividerTwo);
            packets.Add(dividerSix);

            packets
                .Sort();

            var two = packets.IndexOf(dividerTwo) + 1;
            var six = packets.IndexOf(dividerSix) + 1;

            return two * six;
        }

        private List<PacketPair> ReadPackets(string[] input)
        {
            var result = new List<PacketPair>();
            int index = 1;
            Packet? left = null;
            Packet? right = null;
            
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    result.Add(new PacketPair(left!, right!, index));
                    index++;
                    left = null;
                    right = null;
                }
                else if (left is null)
                {
                    left = ParsePacket(line);
                }
                else
                {
                    right = ParsePacket(line);
                }
            }
            result.Add(new PacketPair(left!, right!, index));

            return result;
        }

        private Packet? ParsePacket(string line)
        {
            Packet? result = null;
            Packet? current = null;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                switch(c) 
                {
                    case '[':
                        var inner = new Packet { Parent = current };
                        if (result is null)
                        {
                            result = inner;
                        }
                        if (current is not null)
                        {
                            current.InnerPackets.Add(inner);
                        }                        
                        current = inner;
                        break;
                    case ']':
                        current = current!.Parent;
                        break;
                    case ',':
                        continue;
                    default:
                        var next = line[i + 1];
                        if (next != ',' && next != ']')
                        {
                            current!.InnerPackets.Add(new Packet { Parent = current, Value = int.Parse($"{c}{next}") });
                            i++;
                            
                        }
                        else
                        {
                            current!.InnerPackets.Add(new Packet { Parent = current, Value = int.Parse($"{c}") });
                        }
                        break;
                }
            }
            return result;
        }
    }
}
