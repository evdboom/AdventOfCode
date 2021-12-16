namespace AdventOfCode2021.Constructs.Day10
{
    public class ChunkException : Exception
    {
        public char CorruptChunk { get; init; }

        public ChunkException(char corrupt, char open) : base($"{corrupt} is not a valid closing character for chunk {open}")
        {
            CorruptChunk = corrupt;
        }
    }
}
