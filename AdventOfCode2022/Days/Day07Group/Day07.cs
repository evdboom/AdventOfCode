using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day07Group;

namespace AdventOfCode2022.Days
{
    public class Day07 : Day
    {
        public Day07(IFileImporter importer) : base(importer)
        {

        }

        public override int DayNumber => 7;
        protected override long ProcessPartOne(string[] input)
        {
            var cutoff = 100000;
            var home = GetDirectories(input);
            return GetWithMaxSize(home, cutoff)
                .Sum(d => d.Size);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var totalSpace = 70000000;
            var requiredFreeSpace = 30000000;
            var home = GetDirectories(input);
            var currentFreeSpace = totalSpace - home.Size;                
            var sizeToDelete = requiredFreeSpace - currentFreeSpace;
            return GetWithMinSize(home, sizeToDelete)
                .OrderBy(d => d.Size)
                .FirstOrDefault()?.Size ?? -1;
        }

        private TerminalDir GetDirectories(string[] input)
        {
            var home = new TerminalDir("./");
            var current = home;
            foreach (var line in input)
            {
                if (line.StartsWith("$"))
                {
                    switch (line.Substring(2, 2))
                    {
                        case "cd":
                            current = ChangeDirectory(line, current, home);
                            break;
                        case "ls":
                            break;
                    }
                }
                else if (!line.StartsWith("dir"))
                {
                    var parts = line.Split(' ');
                    current.Files[parts[1]] = long.Parse(parts[0]);
                }
            }

            return home;
        }

        private IEnumerable<TerminalDir> GetWithMaxSize(TerminalDir directory, long maxSize)
        {
            if (directory.Size <= maxSize)
            {
                yield return directory;
            }
            foreach(var sub in directory.Subdirectories)
            {
                foreach(var correct in GetWithMaxSize(sub, maxSize))
                {
                    yield return correct;
                }               
            }
        }
        private IEnumerable<TerminalDir> GetWithMinSize(TerminalDir directory, long minSize)
        {
            if (directory.Size >= minSize)
            {
                yield return directory;
                foreach (var sub in directory.Subdirectories)
                {
                    foreach (var correct in GetWithMinSize(sub, minSize))
                    {
                        yield return correct;
                    }
                }
            }            
        }

        private TerminalDir ChangeDirectory(string line, TerminalDir current, TerminalDir home)
        {
            var dir = line.Split(" cd ")[1];
            return dir switch
            {
                "/" => home,
                ".." => current.Parent!,
                _ => new TerminalDir(dir, current),
            };
        }
    }
}
