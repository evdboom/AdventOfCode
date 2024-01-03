using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day25 : Day
    {
        public Day25(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 25;

        protected override long ProcessPartOne(string[] input)
        {
            var components = GetComponents(input);

            while (true)
            {
                var (count1, count2, connections) = Contract(components);
                if (connections == 3)
                {
                    return count1 * count2;
                }
            }
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return -1;
        }

        private (int Count1, int Count2, int Connections) Contract(Dictionary<string, List<string>> components)
        {
            var toContract = components
                .ToDictionary(component => component.Key, component => component.Value.ToDictionary(connection => connection, _ => 1));
            var stackSize = components
                .ToDictionary(component => component.Key, _ => 1);
            var random = new Random();

            while (toContract.Count > 2)
            {
                var index = random.Next(0, toContract.Count - 1);
                var toMerge = toContract.ElementAt(index);
                var connectionIndex = random.Next(0, toMerge.Value.Count - 1);
                var connectionKey = toMerge.Value.ElementAt(connectionIndex).Key;
                var connection = toContract[connectionKey];
                    
                toContract.Remove(connectionKey);
                toMerge.Value.Remove(connectionKey);
                connection.Remove(toMerge.Key);
                stackSize[toMerge.Key] += stackSize[connectionKey];
                stackSize.Remove(connectionKey);
                foreach(var link in connection)
                {
                    if (toMerge.Value.ContainsKey(link.Key))
                    {
                        toMerge.Value[link.Key] += link.Value;
                    }
                    else
                    {
                        toMerge.Value[link.Key] = link.Value;
                    }
                    toContract[link.Key].Remove(connectionKey);
                    toContract[link.Key][toMerge.Key] = toMerge.Value[link.Key];
                }                
            }

            var first = toContract.First();
            var second = toContract.Last();

            return (stackSize[first.Key], stackSize[second.Key], first.Value[second.Key]);
        }

        private Dictionary<string, List<string>> GetComponents(string[] input)
        {
            var cache = new Dictionary<string, List<string>>();            
            foreach (var line in input)
            {
                var parts = line.Split(": ");
                if (!cache.TryGetValue(parts[0], out var component))
                {
                    component = [];
                    cache[parts[0]] = component;
                }
                var connections = parts[1].Split(' ');
                foreach (var connectionName in connections)
                {
                    if (!component.Contains(connectionName))
                    {
                        component.Add(connectionName);
                    }

                    if (!cache.TryGetValue(connectionName, out var connection))
                    {
                        cache[connectionName] = [parts[0]];
                    }
                    else if (!connection.Contains(parts[0]))
                    {
                        connection.Add(parts[0]);
                    }
                    
                }
            }

            return cache;
        }
    }
}
