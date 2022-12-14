namespace AdventOfCode2022.Days.Day13Group
{
    public class Packet : IComparable<Packet>
    {
        public int? Value { get; set; }
        public List<Packet> InnerPackets { get; } = new();
        public Packet? Parent { get; set; }

        public int CompareTo(Packet? other)
        {
            var pair = new PacketPair(this, other!, 1);
            return pair.IsValid()
                ? -1
                : 1;
        }

        public override string ToString()
        {
            if (Value.HasValue)
            {
                return $"{Value}";
            }
            else if (InnerPackets.Any())
            {
                return $"[{string.Join(',', InnerPackets.Select(x => x.ToString()))}]";
            }
            else
            {
                return "[]";
            }
        }
    }
}
