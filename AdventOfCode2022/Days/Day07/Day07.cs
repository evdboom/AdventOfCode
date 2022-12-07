using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

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
            var directories = GetDirectoriesDictionary(input);
            return directories.Values
                .Where(v => v <= cutoff)
                .Sum();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var directories = GetDirectoriesDictionary(input);
            var sizeToDelete = 30000000 - 70000000 + directories["."];
            return directories.Values
                .Order()
                .First(v => v >= sizeToDelete);
        }

        private Dictionary<string, int> GetDirectoriesDictionary(string[] input)
        {
            return input
                 .Aggregate((new Dictionary<string, int>() { { ".", 0 } }, "."), (value, next) =>
                 {
                     var current = value.Item2;
                     if (next.StartsWith("$ cd "))
                     {
                         var directory = next.Split(" cd ")[1];
                         switch (directory)
                         {
                             case "/":
                                 current = ".";
                                 break;
                             case "..":
                                 current = current.Remove(current.LastIndexOf("/"));
                                 break;
                             default:
                                 current += $"/{directory}";
                                 value.Item1[current] = 0;
                                 break;
                         }
                     }
                     else if (!next.StartsWith("$") && !next.StartsWith("dir"))
                     {
                         var size = int.Parse(next.Split(' ')[0]);
                         var x = current;
                         while (x.Contains("/"))
                         {
                             value.Item1[x] += size;
                             x = x.Remove(x.LastIndexOf("/"));
                         }
                         value.Item1[x] += size;
                     }
                     return (value.Item1, current);
                 }).Item1;
        }
    }
}
