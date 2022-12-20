using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day19Group;

namespace AdventOfCode2022.Days
{
    public class Day19 : Day
    {
        public Day19(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 19;
        protected override long ProcessPartOne(string[] input)
        {
            var blueprints = GetBlueprints(input);
            return blueprints.Aggregate(0L, (value, blueprint) =>
            {
                value += blueprint.Number * GetGeodes(blueprint, 24);
                return value;
            });
        }        

        protected override long ProcessPartTwo(string[] input)
        {
            var blueprints = GetBlueprints(input);
            return blueprints.Take(3).Aggregate(1L, (value, blueprint) =>
            {
                value *= GetGeodes(blueprint, 32);
                return value;
            });
        }

        private int GetGeodes(Blueprint blueprint, int time)
        {
            var initialState = new State { OreBots = 1, TimeRemaining = time };
            var cache = new Dictionary<State, int>();
            _currentMax = 0;
            var result = GetGeodes(blueprint, initialState, cache);            
            return result;
        }

        private int _currentMax = 0;
        private int GetGeodes(Blueprint blueprint, State state, Dictionary<State, int> cache)
        {
            if (state.TimeRemaining == 0)
            {                
                return state.Geodes;
            }

            if (!cache.ContainsKey(state)) 
            {
                var states = GetStates(blueprint, state);
                if (!states.Any())
                {
                    return 0;
                }

                cache[state] = states
                    .Select(s => GetGeodes(blueprint, s, cache))
                    .Max();
                if (cache[state] > _currentMax)
                {
                    _currentMax = cache[state];
                }
            }

            return cache[state];
        }

        private IEnumerable<State> GetStates(Blueprint blueprint, State state)
        {
            var potentialMax = state.Geodes;
            for (int i = 1; i <= state.TimeRemaining; i++) 
            {
                potentialMax += i + state.GeodeBots;
            }
            if (potentialMax < _currentMax)
            {
                yield break;
            }

            if (state.Ore >= blueprint.GeodeBotCosts.Ore && state.Obsidian >= blueprint.GeodeBotCosts.Obsidian)
            {
                yield return state with
                {
                    TimeRemaining = state.TimeRemaining - 1,
                    Ore = state.Ore + state.OreBots - blueprint.GeodeBotCosts.Ore,
                    Clay = state.Clay + state.ClayBots,
                    Obsidian = state.Obsidian + state.ObsidianBots - blueprint.GeodeBotCosts.Obsidian,
                    Geodes = state.Geodes + state.GeodeBots,
                    GeodeBots = state.GeodeBots + 1,
                };   
                yield break;
            }
            if (state.Ore >= blueprint.ObsidianBotCosts.Ore && state.Clay >= blueprint.ObsidianBotCosts.Clay)
            {
                yield return state with
                {
                    TimeRemaining = state.TimeRemaining - 1,
                    Ore = state.Ore + state.OreBots - blueprint.ObsidianBotCosts.Ore,
                    Clay = state.Clay + state.ClayBots - blueprint.ObsidianBotCosts.Clay,
                    Obsidian = state.Obsidian + state.ObsidianBots,
                    Geodes = state.Geodes + state.GeodeBots,
                    ObsidianBots = state.ObsidianBots + 1,
                };
            }
            if (state.Ore >= blueprint.ClayBotCosts)
            {
                yield return state with
                {
                    TimeRemaining = state.TimeRemaining - 1,
                    Ore = state.Ore + state.OreBots - blueprint.ClayBotCosts,
                    Clay = state.Clay + state.ClayBots,
                    Obsidian = state.Obsidian + state.ObsidianBots,
                    Geodes = state.Geodes + state.GeodeBots,
                    ClayBots = state.ClayBots + 1,
                };
            }
            if (state.Ore >= blueprint.OreBotCosts)
            {
                yield return state with
                {
                    TimeRemaining = state.TimeRemaining - 1,
                    Ore = state.Ore + state.OreBots - blueprint.OreBotCosts,
                    Clay = state.Clay + state.ClayBots,
                    Obsidian = state.Obsidian + state.ObsidianBots,
                    Geodes = state.Geodes + state.GeodeBots,
                    OreBots = state.OreBots + 1,
                };
            }
            yield return state with
            {
                TimeRemaining = state.TimeRemaining - 1,
                Ore = state.Ore + state.OreBots,
                Clay = state.Clay + state.ClayBots,
                Obsidian = state.Obsidian + state.ObsidianBots,
                Geodes = state.Geodes + state.GeodeBots,
            };
        }

        private List<Blueprint> GetBlueprints(string[] input) 
        {
            return input
                .Select(ToBlueprint)
                .ToList();
        }

        private Blueprint ToBlueprint(string line)
        {
            var result = new Blueprint();

            var parts = line.Split(": Each ore robot costs ");
            result.Number = int.Parse(parts[0].Replace("Blueprint ", string.Empty));
            parts = parts[1].Split(" ore. Each clay robot costs ");
            result.OreBotCosts = int.Parse(parts[0]);
            parts = parts[1].Split(" ore. Each obsidian robot costs ");
            result.ClayBotCosts = int.Parse(parts[0]);
            parts = parts[1].Split(" clay. Each geode robot costs ");
            var obsidianParts = parts[0].Split(" ore and ");
            result.ObsidianBotCosts = (int.Parse(obsidianParts[0]), int.Parse(obsidianParts[1]));
            var geoParts = parts[1].Replace(" obsidian.", string.Empty).Split(" ore and ");
            result.GeodeBotCosts = (int.Parse(geoParts[0]), int.Parse(geoParts[1]));

            return result;
        }
    }
}