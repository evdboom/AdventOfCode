using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day16Group;
using System.Security.AccessControl;

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
            var distances = GetDistances(valves);

            var initialState = new FlowState
            {
                ClosedValves = valves.Where(valve => valve.Value.FlowRate > 0).Select(valve => valve.Key).ToArray(),
                TimeRemaining = 30,
                Flow = 0,
                Actors = new()
                {
                    {
                        1,
                        new FlowActor
                        {
                            ETA = 30,
                            Id = 1,
                            Location = "AA"
                        }
                    }
                }
            };

            _currentMax = 0;
            var result = GetFlow(initialState, valves, distances);
            return result;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var valves = GetValves(input);
            var distances = GetDistances(valves);

            var initialState = new FlowState
            {
                ClosedValves = valves.Where(valve => valve.Value.FlowRate > 0).Select(valve => valve.Key).ToArray(),
                TimeRemaining = 26,
                Flow = 0,
                Actors = new()
                {
                    {
                        1,
                        new FlowActor
                        {
                            ETA = 26,
                            Id = 1,
                            Location = "AA"
                        }
                    },
                    {
                        2,
                        new FlowActor
                        {
                            ETA = 26,
                            Id = 2,
                            Location = "AA"
                        }
                    }
                }
            };

            _currentMax = 0;
            var result = GetFlow(initialState, valves, distances);
            return result;
        }

        private long _currentMax;
        private long GetFlow(FlowState state, Dictionary<string, Valve> valves, Dictionary<(string, string), int> distances)
        {
            if (state.TimeRemaining == 0 || state.ClosedValves.Length == 0)
            {
                if (state.Flow > _currentMax)
                {
                    _currentMax = state.Flow;
                }
                return state.Flow;
            }

            var potentialMax = GetPotential(state, valves);
            if (potentialMax < _currentMax)
            {
                return 0;
            }

            var max = 0L;

            var finished = state.Actors.Where(actor => actor.Value.ETA == state.TimeRemaining);
            if (finished.Count() == 1)
            {
                var actor = finished.First().Value;
                foreach(var closed in state.ClosedValves)
                {
                    var valve = valves[closed];
                    var distance = distances[(actor.Location, closed)];

                    var eta = state.TimeRemaining - distance - 1;
                    if (eta <= 0)
                    {
                        if (state.Flow > max)
                        {
                            max = state.Flow;
                        }                        
                        continue;
                    }

                    var addedFlow = eta * valve.FlowRate;

                    var actors = state.Actors.ToDictionary(a => a.Key, a => a.Value);
                    actors[actor.Id] = actor with
                    {
                        ETA = eta,
                        Location = closed
                    };
                   
                    var timeRemaining = actors.Max(a => a.Value.ETA);

                    var newState = state with
                    {
                        Actors = actors,
                        ClosedValves = state.ClosedValves.Where(c => c != closed).ToArray(),
                        Flow = state.Flow + addedFlow,
                        TimeRemaining = timeRemaining
                    };
                    var newMax = GetFlow(newState, valves, distances);
                    if (newMax > max)
                    {
                        max = newMax;
                    }
                }
            }
            else if (state.ClosedValves.Length == 1)
            {
                var closed = state.ClosedValves[0];

                var valve = valves[closed];
                var distance1 = distances[(state.Actors[1].Location, closed)];
                var distance2 = distances[(state.Actors[2].Location, closed)];

                var toUse = Math.Min(distance1, distance2);                
                var eta = state.TimeRemaining - toUse - 1;

                if (eta <= 0)
                {
                    if (state.Flow > max)
                    {
                        max = state.Flow;
                    }                 
                }
                else
                {
                    var addedFlow = eta * valve.FlowRate;
                    if (state.Flow + addedFlow > max)
                    {
                        max = state.Flow + addedFlow;
                    }
                }
            }
            else
            {
                foreach (var closed in state.ClosedValves)
                {
                    foreach(var otherClosed in state.ClosedValves)
                    {
                        if (otherClosed == closed)
                        {
                            continue;
                        }

                        var valve1 = valves[closed];
                        var distance1 = distances[(state.Actors[1].Location, closed)];
                        var eta1 = state.TimeRemaining - distance1 - 1;
                        var valve2 = valves[otherClosed];
                        var distance2 = distances[(state.Actors[2].Location, otherClosed)];
                        var eta2 = state.TimeRemaining - distance2 - 1;

                        if (eta1 <= 0 && eta2 <= 0)
                        {
                            if (state.Flow > max)
                            {
                                max = state.Flow;
                            }
                            continue;
                        }

                        var addedFlow = eta1 * valve1.FlowRate + eta2 * valve2.FlowRate;

                        var actors = state.Actors.ToDictionary(a => a.Key, a => a.Value);
                        actors[1] = actors[1] with
                        {
                            ETA = eta1,
                            Location = closed,                            
                        };
                        actors[2] = actors[2] with
                        {
                            ETA = eta2,
                            Location = otherClosed,
                        };

                        var timeRemaining = actors.Max(a => a.Value.ETA);

                        var newState = state with
                        {
                            Actors = actors,
                            ClosedValves = state.ClosedValves.Where(c => c != closed && c != otherClosed).ToArray(),
                            Flow = state.Flow + addedFlow,
                            TimeRemaining = timeRemaining
                        };
                        var newMax = GetFlow(newState, valves, distances);
                        if (newMax > max)
                        {
                            max = newMax;
                        }

                    }                    
                }
            }

            if (max > _currentMax)
            {
                _currentMax = max;
            }

            return max;
        }

        private long GetPotential(FlowState state, Dictionary<string, Valve> valves)
        {
            var closed = state.ClosedValves
                .Select(c => valves[c])
                .OrderByDescending(v => v.FlowRate)
                .ToList();

            var potential = state.Flow;
            var time = state.TimeRemaining;
            while(closed.Any() && time > 0)
            {
                time--;
                var taken = closed.Take(state.Actors.Count).ToList();
                foreach (var valve in taken)
                {
                    potential += time * valve.FlowRate;
                    closed.Remove(valve);
                }                
            }

            return potential;

        }

        private Dictionary<(string, string), int> GetDistances(Dictionary<string, Valve> valves)
        {
            var visited = new Dictionary<string, List<string>>();
            var distances = new Dictionary<(string, string), int>();

            foreach(var valve in valves)
            {
                visited[valve.Key] = new();
                var queue = new PriorityQueue<string, int>();
                distances[(valve.Key, valve.Key)] = 0;
                queue.Enqueue(valve.Key, 0);

                while(queue.TryDequeue(out string? element, out _))
                {
                    if (visited[valve.Key].Contains(element))
                    {
                        continue;
                    }

                    visited[valve.Key].Add(element);
                    foreach(var connection in valves[element].Connections.Split(", "))
                    {
                        if (visited[valve.Key].Contains(connection))
                        {
                            continue;
                        }

                        var value = distances[(valve.Key, element)] + 1;
                        if (!distances.TryGetValue((valve.Key, connection), out int distance) || value < distance)
                        {
                            distances[(valve.Key, connection)] = value;
                        }

                        queue.Enqueue(connection, distances[(valve.Key, connection)]);
                    }
                }
            }

            return distances;
        }

        private Dictionary<string, Valve> GetValves(string[] input)
        {
            var result = new Dictionary<string, Valve>();            
            foreach(var line in input) 
            {
                var parts = line.Split(" has flow rate=");
                var valve = parts[0].Replace("Valve ", string.Empty);
                parts[1] = parts[1].Replace("tunnels lead to valves", "tunnel leads to valve");
                var properties = parts[1].Split("; tunnel leads to valve ");
                result[valve] = new Valve { Name = valve, FlowRate = int.Parse(properties[0]), Connections = properties[1] };                
            }

            return result;
        }
    }
}
