using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day23(IFileImporter importer, IScreenWriter writer) : Day(importer)
    {
        private readonly IScreenWriter _writer = writer;
        private string? _partTwoResult;

        public override int DayNumber => 23;
        public string? PartTwoResult() => _partTwoResult;

        protected override long ProcessPartOne(string[] input)
        {
            var computers = GetComputers(input);
            var cliques = new HashSet<string>();
            foreach (var computer in computers.Keys)
            {
                var currentClique = new Stack<string>();
                currentClique.Push(computer);
                FindCliques(computer, computers, currentClique, cliques, 3, []);
            }

            return cliques
                .Where(c => c.StartsWith('t') || c.Contains(",t"))
                .Count();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var computers = GetComputers(input);
            var cliques = new HashSet<string>();
            foreach (var computer in computers.Keys)
            {
                var currentClique = new Stack<string>();
                currentClique.Push(computer);
                FindCliques(computer, computers, currentClique, cliques, null, []);
            }


            _partTwoResult = cliques.MaxBy(c => c.Length);
            _writer.WriteLine(_partTwoResult ?? "Not found");

            return -1;
                
        }

        private void FindCliques(string computer, Dictionary<string, HashSet<string>> computers, Stack<string> currentClique, HashSet<string> cliques, int? desiredLength, Dictionary<string, List<string>> cache)
        {
            var connected = computers[computer];
            if (desiredLength.HasValue)
            {
                if (currentClique.Count == desiredLength)
                {
                    cliques.Add(string.Join(',', currentClique.Order()));
                    return;
                }                                
            }
            else
            {
                cliques.Add(string.Join(',', currentClique.Order()));
                if (cache.TryGetValue(computer, out var cached) && cached.Count > connected.Count / 2)
                {
                    return;
                }
            }
            
            foreach (var connect in connected)
            {
                if (currentClique.All(c => computers[c].Contains(connect)))
                {
                    currentClique.Push(connect);
                    FindCliques(connect, computers, currentClique, cliques, desiredLength, cache);
                    currentClique.Pop();
                }
            }

            if (!cache.TryGetValue(computer, out var value))
            {
                value = [];
                cache[computer] = value;
            }
            if (currentClique.Count > value.Count)
            {
                cache[computer] = currentClique.ToList();
            }
        }

        private Dictionary<string, HashSet<string>> GetComputers(string[] input)
        {
            var computers = new Dictionary<string, HashSet<string>>();
            foreach (var line in input)
            {
                var parts = line.Split("-");
                var computer = parts[0];
                var connected = parts[1];
                if (!computers.TryGetValue(computer, out HashSet<string>? value))
                {
                    value = [];
                    computers[computer] = value;
                }
                value.Add(connected);
                if (!computers.TryGetValue(connected, out HashSet<string>? connect))
                {
                    connect = [];
                    computers[connected] = connect;
                }
                connect.Add(computer);

            }
            return computers;
        }
    }
}
