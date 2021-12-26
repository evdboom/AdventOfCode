using AdventOfCode2021.Constructs.Day23;
using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day23 : Day
    {
        private readonly IScreenWriter _writer;

        public Day23(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }

        public override int DayNumber => 23;

        protected override long ProcessPartOne(string[] input)
        {
            _writer.Disable();
            var board = GetBoard(input);
            return CalculateMinimum(board);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            _writer.Disable();
            var additional = new Dictionary<int, string>
            {
                [1] = "DCBA",
                [2] = "DBAC"
            };

            var board = GetBoard(input, additional);
            return CalculateMinimum(board);
        }


        private long CalculateMinimum(Board initial)
        {
            PriorityQueue<Board, int> queue = new();
            HashSet<int> visited = new();
            queue.Enqueue(initial, initial.Cost);

            while(queue.TryDequeue(out Board? board, out _))
            {
                PrintBoard(board);
                if (!visited.Add(board.GetHashCode()))
                {
                    continue;
                }
                else if (board.Wins())
                {
                    return board.Cost;
                }

                foreach(var move in board.ValidMoves())
                {
                    queue.Enqueue(move, move.Cost + move.Estimate);
                }                
            }

            throw new InvalidOperationException("Could not find solution");
        }

        private Board GetBoard(string[] input, Dictionary<int, string>? additional = null)
        {
            string hallWay = string.Empty;
            int linesProcessed = 0;
            Dictionary<int, string> rooms = new();
            foreach (var line in input)
            {
                if (line.Any(l => l == Board.Empty))
                {
                    hallWay = line.Replace("#", "");
                        
                }
                else 
                {
                    var convientLine = line.Trim().Replace("#", "");
                    if (string.IsNullOrEmpty(convientLine))
                    {
                        continue;
                    }
                    if (additional != null && linesProcessed == additional.Keys.First())
                    {
                        foreach (var add in additional.Values)
                        {
                            ProcessLine(add, rooms);
                        }
                    }
                    ProcessLine(convientLine, rooms);                    
                    linesProcessed++;
                }
            }

            return new Board(hallWay, rooms[0], rooms[1], rooms[2], rooms[3]);
        }

        private void ProcessLine(string convientLine, Dictionary<int, string> rooms)
        {
            for (int i = 0; i < convientLine.Length; i++)
            {
                if (rooms.ContainsKey(i))
                {
                    rooms[i] += convientLine[i];
                }
                else
                {
                    rooms[i] = $"{convientLine[i]}";
                }
            }
        }

        private void PrintBoard(Board board)
        {
            _writer.WriteLine($"Cost: {board.Cost}, Estimate {board.Estimate}");
            _writer.WriteLine(board.Hallway);
            for (int j = 0; j < board.Rooms.Values.First().Length; j++)
            {
                for (int i = 0; i <= board.Rooms.Keys.Max(); i++)
                {
                    if (board.Rooms.ContainsKey(i))
                    {
                        _writer.Write(board.Rooms[i][j]);
                    }
                    else
                    {
                        _writer.WriteBlock();
                    }
                }
                _writer.NewLine();
            }
            _writer.NewLine();
        }
    }
}
