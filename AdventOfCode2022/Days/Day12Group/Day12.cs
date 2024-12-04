using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day12Group;

namespace AdventOfCode2022.Days
{
    public class Day12 : Day
    {
        public Day12(IFileImporter importer) : base(importer)
        {

        }

        public override int DayNumber => 12;
        protected override long ProcessPartOne(string[] input)
        {
            GetGrid(input, out List<Node> startingNodes, out Node end, 'S');
            var nodes = new PriorityQueue<Node, int>();
            var start = startingNodes.First();
            start.Distance = 0;
            nodes.Enqueue(start, start.Value);
            ProcessNodes(nodes, end);
            return end.Distance;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = GetGrid(input, out List<Node> startingNodes, out Node end, 'S', 'a');
            var min = int.MaxValue;
            foreach(var start in startingNodes)
            {
                start.Distance = 0;
                start.Value = 0;
                var nodes = new PriorityQueue<Node, int>();
                nodes.Enqueue(start, start.Value);
                ProcessNodes(nodes, end);
                if (end.Distance < min)
                {
                    min = end.Distance;
                }

                for (int j = 0; j < grid.Height; j ++)
                {
                    for (int i = 0; i < grid.Width; i++)
                    {
                        grid[i, j].Distance = int.MaxValue;
                        grid[i, j].Visited = false;
                        grid[i, j].Value = 1;
                    }
                }
            }
            
            return min;
        }

        private Grid<Node> GetGrid(string[] input, out List<Node> startingNodes, out Node end, params char[] starting)
        {
            startingNodes = [];
            end = new Node { Value = 1 };
            var result = new Grid<Node>(input[0].Length, input.Length);
            var grid = new Grid<int>(input[0].Length, input.Length);
            for (int j = 0; j < input.Length; j++)
            {
                for (int i = 0; i < input[0].Length; i++)
                {
                    var value = input[j][i];
                    if (starting.Contains(value))
                    {
                        var node = new Node { X = i, Y = j, Value = 1 };                        
                        result[i, j] = node;
                        startingNodes.Add(node);
                    }
                    else if (value == 'E')
                    {
                        value = 'z';
                        end.X = i;
                        end.Y = j;                        
                        result[i, j] = end;                        
                    }
                    else
                    {
                        result[i, j] = new Node { X = i, Y = j, Value = 1 };                        
                    }

                    grid[i, j] = value % 32 - 1;
                }
            }

            for (int j = 0; j < result.Height; j++)
            {
                for (int i = 0; i < result.Width; i++)
                {
                    var current = grid[i, j];
                    foreach (var p in grid.Adjacent(i, j, current + 1))
                    {
                        result[i,j].Connections.Add(result[p.X, p.Y]);
                    }
                }
            }

            return result;
        }

        private void ProcessNodes(PriorityQueue<Node, int> nodes, Node wanted)
        {
            while (nodes.TryDequeue(out Node? node, out _))
            {
                if (node.Visited)
                {
                    continue;
                }

                if (node == wanted)
                {
                    break;
                }

                node.Visited = true;
                foreach (var connection in node.Connections.Where(c => !c.Visited))
                {
                    var value = node.Distance + connection.Value;
                    if (value < connection.Distance)
                    {
                        connection.Distance = value;
                    }

                    nodes.Enqueue(connection, connection.Distance);
                }
            }
        }        
    }
}
