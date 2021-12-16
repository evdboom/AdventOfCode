namespace AdventOfCode2021.Constructs.Day16
{
    public class StringStream
    {
        private readonly string _binary;

        private int _position;

        public StringStream(string binary)
        {
            _binary = binary;
        }

        public string ReadString(int length)
        {
            var result = _binary.Substring(_position, length);
            _position += length;

            return result;
        }

        public int ReadInt(int length)
        {
            var result = ReadString(length);
            return Convert.ToInt32(result, 2);
        }

        public int GetPosition()
        {
            return _position;
        }
    }
}
