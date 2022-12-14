namespace AdventOfCode2022.Days.Day13Group
{
    internal class PacketPair
    {
        public int Index { get; }
        public Packet Left { get; }
        public Packet Right { get; }

        public PacketPair(Packet left, Packet right, int index)
        {
            Left = left;
            Right = right;
            Index = index;
        }

        public bool IsValid()
        {
            return Compare(Left, Right) ?? false;
        }

        private bool? Compare(Packet left, Packet right)
        {
            if (left.Value.HasValue && !right.Value.HasValue)
            {
                left.InnerPackets.Add(new Packet { Parent = left, Value = left.Value });
                left.Value = null;
            }
            else if (!left.Value.HasValue && right.Value.HasValue)
            {
                right.InnerPackets.Add(new Packet { Parent = right, Value = right.Value });
                right.Value = null;
            }
            
            if (left.Value.HasValue && right.Value.HasValue)
            {
                if (left.Value == right.Value) 
                {
                    return null;
                }
                else
                {
                    return left.Value < right.Value;
                }
            }
            else
            {
                for (int i = 0; (i < left.InnerPackets.Count && i < right.InnerPackets.Count); i++) 
                {
                    var result = Compare(left.InnerPackets[i], right.InnerPackets[i]);
                    if (result.HasValue)
                    {
                        return result;
                    }
                }
                return left.InnerPackets.Count == right.InnerPackets.Count
                    ? null
                    : left.InnerPackets.Count < right.InnerPackets.Count;
            }
        }
    }
}
