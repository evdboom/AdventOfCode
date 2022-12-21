using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day21 : Day
    {
        public Day21(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 21;
        protected override long ProcessPartOne(string[] input)
        {
            var monkeys = input
                .ToDictionary(line => line.Split(":")[0], line => line.Split(":")[1].Trim());

            return GetReturn("root", monkeys);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var monkeys = input
                .ToDictionary(line => line.Split(":")[0], line => line.Split(":")[1].Trim());

            var formula = GetFormula("root", monkeys);
            var parts = formula.Split(" = ");
            if (long.TryParse(parts[0], out long value))
            {
                return DecodeFormula(parts[1], value);
            }
            else
            {
                return DecodeFormula(parts[0], long.Parse(parts[1]));
            }
        }

        private long DecodeFormula(string formula, long value)
        {
            if (formula.StartsWith("(") && formula.EndsWith(")"))
            {
                formula = formula[1..^1];
            }

            if (formula.Contains("("))
            {
                var inner = formula.StartsWith("(")
                ? formula[..(formula.LastIndexOf(")") + 1)]
                : formula[formula.IndexOf("(")..];

                var other = formula.Replace(inner, string.Empty).Trim();
                value = GetNewValue(other, value);

                return DecodeFormula(inner, value);
            }
            else
            {
                var other = formula.Replace("humn", string.Empty).Trim();
                return GetNewValue(other, value);
            }
        }

        private long GetNewValue(string other, long value)
        {
            if (other.StartsWith("+") || other.EndsWith("+"))
            {
                value -= long.Parse(other.Replace("+", string.Empty).Trim());
            }
            else if (other.StartsWith("*") || other.EndsWith("*"))
            {
                value /= long.Parse(other.Replace("*", string.Empty).Trim());
            }
            else if (other.StartsWith("-"))
            {
                value += long.Parse(other.Replace("- ", string.Empty));
            }
            else if (other.EndsWith("-"))
            {
                value -= long.Parse(other.Replace(" -", string.Empty));
                value *= -1;
            }
            else if (other.StartsWith("/"))
            {
                value *= long.Parse(other.Replace("/ ", string.Empty));
            }
            else if (other.EndsWith("/"))
            {
                value = long.Parse(other.Replace(" /", string.Empty)) / value;
            }

            return value;
        }

        private string GetFormula(string monkey, Dictionary<string, string> monkeys)
        {
            var action = monkeys[monkey];
            if (monkey == "humn")
            {
                return monkey;
            }
            if (long.TryParse(action, out _))
            {
                return action;
            }

            var parts = action.Split(" ");
            var monkey1 = GetFormula(parts[0], monkeys);
            var monkey2 = GetFormula(parts[2], monkeys);

            if (monkey == "root")
            {
                return $"{monkey1} = {monkey2}";
            }
            else if (long.TryParse(monkey1, out long result1) && long.TryParse(monkey2, out long result2))
            {
                return parts[1] switch
                {
                    "+" => $"{result1 + result2}",
                    "*" => $"{result1 * result2}",
                    "-" => $"{result1 - result2}",
                    "/" => $"{result1 / result2}",
                    _ => throw new InvalidOperationException("unknown operator")
                };
            }

            return parts[1] switch
            {
                "+" => $"({monkey1} + {monkey2})",
                "*" => $"({monkey1} * {monkey2})",
                "-" => $"({monkey1} - {monkey2})",
                "/" => $"({monkey1} / {monkey2})",
                _ => throw new InvalidOperationException("unknown operator")
            };
        }

        private long GetReturn(string monkey, Dictionary<string, string> monkeys)
        {
            var action = monkeys[monkey];
            if (long.TryParse(action, out long result))
            {
                return result;
            }

            var parts = action.Split(" ");
            var monkey1 = GetReturn(parts[0], monkeys);
            var monkey2 = GetReturn(parts[2], monkeys);

            return parts[1] switch
            {
                "+" => monkey1 + monkey2,
                "*" => monkey1 * monkey2,
                "-" => monkey1 - monkey2,
                "/" => monkey1 / monkey2,
                _ => throw new InvalidOperationException("unknown operator")
            };
        }
    }
}