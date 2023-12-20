using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day19Group;
using System.Data;

namespace AdventOfCode2023.Days
{
    public class Day19 : Day
    {
        public Day19(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 19;

        protected override long ProcessPartOne(string[] input)
        {
            var split = input
                .Select((line, index) => (Line: line, Index: index))
                .FirstOrDefault(line => string.IsNullOrEmpty(line.Line)).Index;

            var workFlows = GetWorkFlows(input.Where((line, index) => index < split))
                .ToDictionary(flow => flow.Name, flow => flow);
            return GetParts(input.Where((line, index) => index > split))
                .Select(part => GetFlowResult("in", workFlows, part))
                .Where(result => result.Result == "A")
                .Sum(result => result.Value);

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var split = input
                .Select((line, index) => (Line: line, Index: index))
                .FirstOrDefault(line => string.IsNullOrEmpty(line.Line)).Index;

            var workFlows = GetWorkFlows(input.Where((line, index) => index < split))
                .ToDictionary(flow => flow.Name, flow => flow);

            return GetRangeResult("in", workFlows, new())
                .Sum(ranges => (long)ranges.X.Count * ranges.M.Count * ranges.A.Count * ranges.S.Count);                
        }

        private IEnumerable<Ranges> GetRangeResult(string workFlow, Dictionary<string, WorkFlow> workFlows, Ranges ranges)
        {
            if (!workFlows.TryGetValue(workFlow, out var flow))
            {
                if (workFlow == "A")
                {
                    yield return ranges;
                }
                yield break;
            }

            var options = GetFlowOptions(flow, ranges);

            foreach(var (newFlow, newRanges) in options)
            {
                foreach(var result in GetRangeResult(newFlow, workFlows, newRanges))
                {
                    yield return result;
                }           
            }
        }

        private IEnumerable<(string Flow, Ranges Ranges)> GetFlowOptions(WorkFlow flow, Ranges ranges)
        {
            ranges = ranges.Copy();
            foreach (var rule in flow.Rules)
            {
                var newRanges = ranges.Copy();
                switch (rule.RatingType)
                {
                    case 'x':                        
                        newRanges.X = GetNewRange(rule.Value, rule.Operator, newRanges.X);
                        ranges.X = ranges.X.Except(newRanges.X).ToList();
                        yield return (rule.Result, newRanges);
                        break;
                    case 'm':                        
                        newRanges.M = GetNewRange(rule.Value, rule.Operator, newRanges.M);
                        ranges.M = ranges.M.Except(newRanges.M).ToList();
                        yield return (rule.Result, newRanges);
                        break;
                    case 'a':                        
                        newRanges.A = GetNewRange(rule.Value, rule.Operator, newRanges.A);
                        ranges.A = ranges.A.Except(newRanges.A).ToList();
                        yield return (rule.Result, newRanges);
                        break;
                    case 's':                        
                        newRanges.S = GetNewRange(rule.Value, rule.Operator, newRanges.S);
                        ranges.S = ranges.S.Except(newRanges.S).ToList();
                        yield return (rule.Result, newRanges);
                        break;
                }
            }

            yield return (flow.DefaultResult, ranges);
        }

        private List<int> GetNewRange(int ruleValue, char ruleOperator, IEnumerable<int> range)
        {
            return ruleOperator == '>'
                ? range.Where(value => value > ruleValue).ToList()
                : range.Where(value => value < ruleValue).ToList();
        }

        private (string Result, long Value) GetFlowResult(string workFlow, Dictionary<string, WorkFlow> workFlows, (int X, int M, int A, int S) part)
        {
            if (!workFlows.TryGetValue(workFlow, out var flow))
            {
                return (workFlow, part.X + part.M + part.A + part.S);
            }

            var newFlow = ProcessFlow(part, flow);
            return GetFlowResult(newFlow, workFlows, part);
        }

        private string ProcessFlow((int X, int M, int A, int S) part, WorkFlow flow)
        {
            foreach (var rule in flow.Rules)
            {
                switch (rule.RatingType)
                {
                    case 'x':
                        if (RuleApplies(rule.Value, rule.Operator, part.X))
                        {
                            return rule.Result;
                        }
                        break;
                    case 'm':
                        if (RuleApplies(rule.Value, rule.Operator, part.M))
                        {
                            return rule.Result;
                        }
                        break;
                    case 'a':
                        if (RuleApplies(rule.Value, rule.Operator, part.A))
                        {
                            return rule.Result;
                        }
                        break;
                    case 's':
                        if (RuleApplies(rule.Value, rule.Operator, part.S))
                        {
                            return rule.Result;
                        }
                        break;
                }
            }

            return flow.DefaultResult;
        }

        private bool RuleApplies(int ruleValue, char ruleOperator, int partValue)
        {
            return
                (ruleOperator == '>' && partValue > ruleValue) ||
                (ruleOperator == '<' && partValue < ruleValue);
        }

        private IEnumerable<WorkFlow> GetWorkFlows(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                var parts = line.Split('{');
                var result = new WorkFlow
                {
                    Name = parts[0]
                };

                var flows = parts[1].Split(',');

                foreach (var flow in flows)
                {
                    if (flow.Contains('}'))
                    {
                        result.DefaultResult = flow[..^1];
                        continue;
                    }

                    var flowParts = flow.Split(':');
                    result.Rules.Add(new WorkFlowRule
                    {
                        RatingType = flowParts[0][0],
                        Operator = flowParts[0][1],
                        Value = int.Parse(flowParts[0][2..]),
                        Result = flowParts[1]
                    });

                }

                yield return result;
            }
        }

        private IEnumerable<(int X, int M, int A, int S)> GetParts(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                var parts = line[1..^1]
                        .Split(',')
                        .Select(part => int.Parse(part.Split('=')[1]))
                        .ToArray();
                yield return (parts[0], parts[1], parts[2], parts[3]);
            }
        }
    }
}