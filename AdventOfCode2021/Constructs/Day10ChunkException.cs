namespace AdventOfCode2021.Constructs
{
    public class Day10ChunkException : Exception
    {
        public char CorruptChunk { get; init; }

        public Day10ChunkException(char corrupt, char open) : base($"{corrupt} is not a valid closing character for chunk {open}")
        {
            CorruptChunk = corrupt;
        }
    }
}
