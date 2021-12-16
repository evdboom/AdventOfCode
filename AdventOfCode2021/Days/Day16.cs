using AdventOfCode2021.Constructs;
using AdventOfCode2021.Constructs.Day16;
using AdventOfCode2021.Enums;
using AdventOfCode2021.Enums.Day16;
using AdventOfCode2021.Services;
using System.Text;

namespace AdventOfCode2021.Days
{
    public class Day16 : Day
    {
        public Day16(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 16;

        protected override long ProcessPartOne(string[] input)
        {
            var packet = GetPacket(input);
            return packet.GetVersionSum();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var packet = GetPacket(input);
            return packet.GetValue();
        }

        private Packet GetPacket(string[] input)
        {
            var binary = string.Join(string.Empty, input[0]
                .Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            var stream = new StringStream(binary);
            return ReadPacket(stream);
        }

        private Packet ReadPacket(StringStream stream)
        {            
            var version = stream.ReadInt(Packet.VersionLength);            
            var type = (PacketType)stream.ReadInt(Packet.TypeLength);

            var result = new Packet(version, type);
            
            if (result.IsLiteralType)
            {
                ReadLiteral(result, stream);
            }
            else
            {
                ReadOperator(result, stream);
            }            

            return result;
        }

        private void ReadOperator(Packet packet, StringStream stream)
        {
            var type = stream.ReadString(1);           
            var length = GetLength(type, stream);

            if (type == Packet.LengthTypeCount)
            {
                for (int i = 0; i < length; i++)
                {
                    var subPacket = ReadPacket(stream);
                    packet.SubPackets.Add(subPacket);
                }
            }
            else
            {
                var currentPosition = stream.GetPosition();
                while (stream.GetPosition() < currentPosition + length)
                {
                    var subPacket = ReadPacket(stream);
                    packet.SubPackets.Add(subPacket);
                }
            }
        }

        private int GetLength(string type, StringStream stream)
        {
            return type == Packet.LengthTypeSize
                ? stream.ReadInt(Packet.LengthTypeSizeLength)
                : stream.ReadInt(Packet.LengthTypeCountLength);           
        }

        private void ReadLiteral(Packet currentPacket, StringStream stream)
        {
            StringBuilder resultBuilder = new();
            var hasNext = true;
            while (hasNext)
            {
                var value = ReadNext(stream, out hasNext);
                resultBuilder.Append(value);
            }

            currentPacket.LiteralValue = Convert.ToInt64(resultBuilder.ToString(), 2);
        }

        private string ReadNext(StringStream stream, out bool hasNext)
        {
            var next = stream.ReadString(Packet.LiteralGroupLength);
            
            hasNext = next[0] != Packet.LastLiteralGroup;
            return next[1..];
        }
    }
}
