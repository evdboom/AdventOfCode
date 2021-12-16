using AdventOfCode2021.Enums;

namespace AdventOfCode2021.Constructs
{
    public class Day16Packet
    {
        public const int VersionLength = 3;
        public const int TypeLength = 3;

        public const int LengthTypeSizeLength = 15;
        public const int LengthTypeCountLength = 11;

        public const string LengthTypeSize = "0";
        public const string LengthTypeCount = "1";

        public const int LiteralGroupLength = 5;
        public const char LastLiteralGroup = '0';

        public int Version { get; init; }
        public Day16PacketType Type { get; init; }

        public long LiteralValue { get; set; }

        public bool IsLiteralType => Type == Day16PacketType.Literal;

        public IList<Day16Packet> SubPackets { get; }

        public Day16Packet(int version, Day16PacketType type)
        {
            Version = version;
            Type = type;
            SubPackets = new List<Day16Packet>();
        }

        public long GetVersionSum()
        {
            return Version + SubPackets.Sum(p => p.GetVersionSum());
        }

        public long GetValue()
        {
            return Type switch
            {
                Day16PacketType.Sum => SubPackets.Sum(p => p.GetValue()),
                Day16PacketType.Product => SubPackets.Select(p => p.GetValue()).Aggregate((a,b) => a * b),
                Day16PacketType.Minimum => SubPackets.Min(p => p.GetValue()),
                Day16PacketType.Maximum => SubPackets.Max(p => p.GetValue()),
                Day16PacketType.Literal => LiteralValue,
                Day16PacketType.GreaterThen => SubPackets[0].GetValue() > SubPackets[1].GetValue() ? 1 : 0,
                Day16PacketType.LessThen => SubPackets[0].GetValue() < SubPackets[1].GetValue() ? 1 : 0,
                Day16PacketType.EqualTo => SubPackets[0].GetValue() == SubPackets[1].GetValue() ? 1 : 0,
                _ => throw new NotImplementedException()
            };
        }

    }
}
