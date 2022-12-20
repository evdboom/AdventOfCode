using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day16Group;

namespace AdventOfCode2022.Days
{
    public class Day16 : Day
    {
        public Day16(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 16;
        protected override long ProcessPartOne(string[] input)
        {
            var valves = GetValves(input);
            foreach(var valve in valves) 
            {
                var queue = new PriorityQueue<Valve, int>();
                valve.Distances[valve.Name] = 0;
                valve.Paths[valve.Name] = new();
                queue.Enqueue(valve, 0);
                ProcessNodes(queue, valve.Name);
            }
            var maxFlow = valves
                .Select(valve => (30 - valve.Distances["AA"] - 1) * valve.FlowRate)
                .ToList();


            return -1;
        }

        private void ProcessNodes(PriorityQueue<Valve, int> valves, string start)
        {
            while (valves.TryDequeue(out Valve? valve, out _))
            {
                if (valve.Visited.TryGetValue(start, out bool visited) && visited)
                {
                    continue;
                }

                valve.Visited[start] = true;
                foreach (var connection in valve.Connections.Where(c => !(c.Visited.TryGetValue(start, out bool cv) && cv)))
                {
                    var value = valve.Distances[start] + 1;
                    if (!connection.Distances.ContainsKey(start) || value < connection.Distances[start])
                    {
                        connection.Distances[start] = value;
                        connection.Paths[start] = new();
                        connection.Paths[start].AddRange(valve.Paths[start]);
                        connection.Paths[start].Add(valve);
                    }

                    valves.Enqueue(connection, connection.Distances[start]);
                }
            }
        }

        private List<Valve> GetValves(string[] input)
        {
            var result = new Dictionary<string, Valve>();
            var connections = new Dictionary<string, string>();
            foreach(var line in input) 
            {
                var parts = line.Split(" has flow rate=");
                var valve = parts[0].Replace("Valve ", string.Empty);
                parts[1] = parts[1].Replace("tunnels lead to valves", "tunnel leads to valve");
                var properties = parts[1].Split("; tunnel leads to valve ");
                result[valve] = new Valve { Name = valve, FlowRate = int.Parse(properties[0]) };
                connections[valve] = properties[1];
            }
            foreach(var valve in result)
            {
                var toValves = connections[valve.Key].Split(", ");
                foreach(var to in toValves)
                {
                    valve.Value.Connections.Add(result[to]);
                }
            }

            return result.Values.ToList();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return -1;
        }

    }
}
