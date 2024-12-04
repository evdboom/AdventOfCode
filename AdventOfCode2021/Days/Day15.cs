using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using AdventOfCode2021.Constructs.Day15;

namespace AdventOfCode2021.Days
{
    public class Day15 : Day
    {

        public Day15(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 15;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToIntGrid();
            return Processday(grid);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToIntGrid();
            var largeGrid = EnlargeGrid(grid, 5);
            return Processday(largeGrid);
        }

        private Grid<int> EnlargeGrid(Grid<int> grid, int factor)
        {
            var oldWith = grid.Width;
            var oldHeight = grid.Height;
            var width = oldWith * factor;
            var heigth = oldHeight * factor;

            var largeGrid = new Grid<int>(width, heigth);

            for (int y = 0; y < factor; y++)
            {
                for (int x = 0; x < factor; x++)
                {
                    for (int j = 0; j < grid.Height; j++)
                    {
                        for (int i = 0; i < grid.Width; i++)
                        {
                            var value = grid[i, j] + y + x;
                            if (value > 9)
                            {
                                value -= 9;
                            }
                            largeGrid[i + (oldWith * x), j + (oldHeight * y)] = value;
                        }
                    }

                }
            }

            return largeGrid;
        }

        private long Processday(Grid<int> grid)
        {
            var nodeGrid = new Node[grid.Width, grid.Height];
            for (int j = 0; j < grid.Height; j++)
            {
                for (int i = 0; i < grid.Width; i++)
                {
                    var newNode = new Node { X = i, Y = j, Value = grid[i, j] };
                    nodeGrid[i, j] = newNode;
                }
            }

            for (int j = 0; j < grid.Height; j++)
            {
                for (int i = 0; i < grid.Width; i++)
                {
                    var current = nodeGrid[i, j];
                    foreach (var p in grid.Adjacent(i, j))
                    {
                        current.Connections.Add(nodeGrid[p.X, p.Y]);
                    }
                }
            }

            Node wanted = nodeGrid[grid.Width - 1, grid.Height - 1];
            Node start = nodeGrid[0, 0];
            start.Distance = 0;
            start.Value = 0;
            var nodes = new PriorityQueue<Node, int>();

            nodes.Enqueue(start, start.Value);
            ProcessNodes(nodes, wanted);

            return wanted.Distance;
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
