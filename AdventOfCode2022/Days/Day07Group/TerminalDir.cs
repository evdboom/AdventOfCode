namespace AdventOfCode2022.Days.Day07Group
{
    public class TerminalDir
    {
        public TerminalDir? Parent { get; }
        public string Path { get; }
        public Dictionary<string, long> Files { get; } = new();
        public List<TerminalDir> Subdirectories { get; } = new();
        public long Size => GetSize();

        public TerminalDir(string path)
        {
            Path = path;
        }

        public TerminalDir(string path, TerminalDir parent)
        {
            Path = path;
            Parent = parent;
            Parent.Subdirectories.Add(this);
        }

        public long GetSize()
        {
            return Files.Values.Sum() + Subdirectories.Select(d => d.GetSize()).Sum();
        }
    }
}
