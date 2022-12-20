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
            return GetFlowRate("AA", 30, valves, false);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var valves = GetValves(input);
            return GetFlowRate("AA", 26, valves, true);
        }

        private long _currentMax;
        private long GetFlowRate(string start, int timeRemaining, Dictionary<string, Valve> valves, bool useElephant)
        {
            _currentMax = 0;
            var initialState = new State
            {              
                Location = start,
                TimeRemaining = timeRemaining
            };
            if (useElephant)
            {
                initialState.Elephant = start;
            }
            var cache = new Dictionary<State, long>();

            return GetFlowRate(initialState, cache, valves);
        }

        private long GetFlowRate(State state, Dictionary<State, long> cache, Dictionary<string, Valve> valves)
        {
            if (state.TimeRemaining == 0)
            {
                return state.Flow;
            }

            if (!cache.ContainsKey(state))
            {
                var states = GetStates(state, valves);
                if (!states.Any())
                {
                    return 0;
                }

                cache[state] = states
                    .Select(s => GetFlowRate(s, cache, valves))
                    .Max();
                if (cache[state] > _currentMax)
                {
                    _currentMax = cache[state];
                }
            }

            return cache[state];
        }

        private IEnumerable<State> GetStates(State state, Dictionary<string, Valve> valves)
        {
            var potentialMax = state.Flow;
            var flowRate = state.FlowRate;
            var open = state.OpenValves.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < state.TimeRemaining; i++)
            {
                potentialMax += flowRate;
                var closed = valves
                    .Where(valve => valve.Value.FlowRate > 0)
                    .Where(valve => !open.Contains(valve.Key))
                    .OrderByDescending(valve => valve.Value.FlowRate)
                    .Select(valve => valve.Value)
                    .FirstOrDefault();
                if (closed is not null)
                {
                    flowRate += closed.FlowRate;
                    open.Add(closed.Name);
                }
                if (!string.IsNullOrEmpty(state.Elephant))
                {
                    closed = valves
                        .Where(valve => valve.Value.FlowRate > 0)
                        .Where(valve => !open.Contains(valve.Key))
                        .OrderByDescending(valve => valve.Value.FlowRate)
                        .Select(valve => valve.Value)
                        .FirstOrDefault();
                    if (closed is not null)
                    {
                        flowRate += closed.FlowRate;
                        open.Add(closed.Name);
                    }
                }
            }

            if (potentialMax < _currentMax)
            {
                yield break;
            }

            var maxOpenCount = valves
                    .Count(valve => valve.Value.FlowRate > 0);

            if (state.OpenValves.Split(",", StringSplitOptions.RemoveEmptyEntries).Length == maxOpenCount)
            {
                var baseFlow = state.Flow;
                for (int i = 0; i < state.TimeRemaining; i++)
                {
                    baseFlow += state.FlowRate;
                }
                yield return state with
                {
                    Flow = baseFlow,
                    TimeRemaining = 0
                };
                yield break;
            }

            foreach (var innerState in GetStates(state, false, valves))
            {
                if (!string.IsNullOrEmpty(state.Elephant))
                {
                    foreach(var deepState in GetStates(state, true, valves))
                    {
                        var openValves = (deepState.OpenValves + "," + innerState.OpenValves).Split(",", StringSplitOptions.RemoveEmptyEntries).Distinct();
                        yield return innerState with
                        {
                            Elephant = deepState.Elephant,
                            PreviousElephant = deepState.PreviousElephant,
                            OpenValves = string.Join(",", openValves),
                            FlowRate = openValves.Select(v => valves[v].FlowRate).Sum()
                        };
                    }
                }
                else
                {
                    yield return innerState;
                }
            }

        }

        private IEnumerable<State> GetStates(State state, bool elephant, Dictionary<string, Valve> valves)
        {
            if ((!elephant || state.Elephant != state.Location) && !state.OpenValves.Split(",", StringSplitOptions.RemoveEmptyEntries).Contains(elephant ? state.Elephant : state.Location) && (elephant ? valves[state.Elephant].FlowRate : valves[state.Location].FlowRate) > 0)
            {
                var newState = state with
                {
                    OpenValves = state.OpenValves + "," + (elephant ? state.Elephant : state.Location),
                    Flow = state.Flow + state.FlowRate,
                    FlowRate = state.FlowRate + (elephant ? valves[state.Elephant].FlowRate : valves[state.Location].FlowRate),
                    TimeRemaining = state.TimeRemaining - 1,
                    Previous = elephant ? state.Previous : null,
                    PreviousElephant = elephant ? null : state.PreviousElephant
                };                
                yield return newState;
            }

            foreach (var connection in (elephant ? valves[state.Elephant].Connections.Split(", ") : valves[state.Location].Connections.Split(", ")))
            {
                if (connection == (elephant ? state.PreviousElephant : state.Previous))
                {
                    continue;
                }

                yield return state with
                {
                    Location = elephant ? state.Location : connection,
                    Previous = elephant ? state.Previous : state.Location,
                    Elephant = elephant ? connection : state.Elephant,
                    PreviousElephant = elephant ? state.Elephant : state.PreviousElephant,
                    Flow = state.Flow + state.FlowRate,
                    TimeRemaining = state.TimeRemaining - 1,
                };
            }
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
