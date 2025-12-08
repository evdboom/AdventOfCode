using System.Drawing;
using System.Numerics;
using AdventOfCode.Shared;
using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days;

public class Day08(IFileImporter fileImporter) : Day(fileImporter)
{
    public override int DayNumber => 8;
    public int Steps { get; set; } = 1000;

    protected override long ProcessPartOne(string[] input)
    {
        var points = ParseInput(input).OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Z).ToList();

        var distances = new PriorityQueue<(Point3D First, Point3D Second), double>();

        for (var i = 0; i < points.Count; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                var first = points[i];
                var second = points[j];
                var distance = Math.Sqrt(
                    Math.Pow(first.X - second.X, 2)
                        + Math.Pow(first.Y - second.Y, 2)
                        + Math.Pow(first.Z - second.Z, 2)
                );
                distances.Enqueue((first, second), distance);
            }
        }

        var circuits = new List<HashSet<Point3D>>();

        for (int i = 0; i < Steps; i++)
        {
            if (distances.TryDequeue(out var pair, out _))
            {
                var (first, second) = pair;
                // option 1: neither point is in a circuit => add a cicruit with both points
                // option 2: one point is in a circuit => add the other point to that circuit
                // option 3: both points are in different circuits => merge the circuits

                var firstCircuit = circuits.FirstOrDefault(c => c.Contains(first));
                var secondCircuit = circuits.FirstOrDefault(c => c.Contains(second));
                if (firstCircuit == null && secondCircuit == null)
                {
                    var newCircuit = new HashSet<Point3D> { first, second };
                    circuits.Add(newCircuit);
                }
                else if (firstCircuit != null && secondCircuit == null)
                {
                    firstCircuit.Add(second);
                }
                else if (firstCircuit == null && secondCircuit != null)
                {
                    secondCircuit.Add(first);
                }
                else if (firstCircuit != secondCircuit)
                {
                    // merge circuits
                    firstCircuit!.UnionWith(secondCircuit!);
                    circuits.Remove(secondCircuit!);
                }
            }
            else
            {
                break; // No more pairs to process
            }
        }

        return circuits
            .OrderByDescending(s => s.Count)
            .Take(3)
            .Aggregate(1L, (acc, curr) => acc * curr.Count);
    }

    protected override long ProcessPartTwo(string[] input)
    {
        var points = ParseInput(input).OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Z).ToList();

        var distances = new PriorityQueue<(Point3D First, Point3D Second), double>();

        for (var i = 0; i < points.Count; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                var first = points[i];
                var second = points[j];
                var distance = Math.Sqrt(
                    Math.Pow(first.X - second.X, 2)
                        + Math.Pow(first.Y - second.Y, 2)
                        + Math.Pow(first.Z - second.Z, 2)
                );
                distances.Enqueue((first, second), distance);
            }
        }

        var circuits = new List<HashSet<Point3D>>();

        while (distances.TryDequeue(out var pair, out _))
        {
            var (first, second) = pair;
            // option 1: neither point is in a circuit => add a cicruit with both points
            // option 2: one point is in a circuit => add the other point to that circuit
            // option 3: both points are in different circuits => merge the circuits

            var firstCircuit = circuits.FirstOrDefault(c => c.Contains(first));
            var secondCircuit = circuits.FirstOrDefault(c => c.Contains(second));
            if (firstCircuit == null && secondCircuit == null)
            {
                var newCircuit = new HashSet<Point3D> { first, second };
                circuits.Add(newCircuit);
            }
            else if (firstCircuit != null && secondCircuit == null)
            {
                firstCircuit.Add(second);
            }
            else if (firstCircuit == null && secondCircuit != null)
            {
                secondCircuit.Add(first);
            }
            else if (firstCircuit != secondCircuit)
            {
                // merge circuits
                firstCircuit!.UnionWith(secondCircuit!);
                circuits.Remove(secondCircuit!);
            }

            if (circuits.Count == 1 && circuits[0].Count == points.Count)
            {
                return (long)first.X * second.X;
            }
        }

        return 0;
    }

    private static List<Point3D> ParseInput(string[] input)
    {
        return
        [
            .. input.Select(line =>
            {
                var parts = line.Split(',');
                return new Point3D(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
            }),
        ];
    }
}
