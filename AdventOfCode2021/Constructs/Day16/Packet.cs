using AdventOfCode2021.Enums.Day16;

namespace AdventOfCode2021.Constructs.Day16
{
    public class Packet
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
        public PacketType Type { get; init; }

        public long LiteralValue { get; set; }

        public bool IsLiteralType => Type == PacketType.Literal;

        public IList<Packet> SubPackets { get; }

        public Packet(int version, PacketType type)
        {
            Version = version;
            Type = type;
            SubPackets = new List<Packet>();
        }

        public long GetVersionSum()
        {
            return Version + SubPackets.Sum(p => p.GetVersionSum());
        }

        public long GetValue()
        {
            return Type switch
            {
                PacketType.Sum => SubPackets.Sum(p => p.GetValue()),
                PacketType.Product => SubPackets.Select(p => p.GetValue()).Aggregate((a, b) => a * b),
                PacketType.Minimum => SubPackets.Min(p => p.GetValue()),
                PacketType.Maximum => SubPackets.Max(p => p.GetValue()),
                PacketType.Literal => LiteralValue,
                PacketType.GreaterThen => SubPackets[0].GetValue() > SubPackets[1].GetValue() ? 1 : 0,
                PacketType.LessThen => SubPackets[0].GetValue() < SubPackets[1].GetValue() ? 1 : 0,
                PacketType.EqualTo => SubPackets[0].GetValue() == SubPackets[1].GetValue() ? 1 : 0,
                _ => throw new NotImplementedException()
            };
        }

    }
}
