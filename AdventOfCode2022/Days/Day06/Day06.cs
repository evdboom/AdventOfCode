using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day06 : Day
    {
        private const int PacketStart = 4;
        private const int MessageStart = 14;

        public Day06(IFileImporter importer) : base(importer)
        {

        }

        public override int DayNumber => 6;
        protected override long ProcessPartOne(string[] input)
        {
            return GetPacket(input[0], PacketStart);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return GetPacket(input[0], MessageStart);            
        }

        private long GetPacket(string stream, int uniquePacketSize)
        {
            for (int i = 0; i < stream.Length - uniquePacketSize; i++)
            {
                var uniquePacket = stream
                    .Substring(i, uniquePacketSize)
                    .Distinct()
                    .Count();
                if (uniquePacket == uniquePacketSize)
                {
                    return i + uniquePacketSize;
                }
            }

            throw new InvalidOperationException("No packet start found");
        }
    }
}
