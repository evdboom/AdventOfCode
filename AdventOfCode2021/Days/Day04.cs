using AdventOfCode.Shared.Days;
using AdventOfCode2021.Constructs.Day04;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2021.Days
{
    public class Day04 : Day
    {
        private const char NumberSplit = ',';
        private const char ValueSplit = ' ';
        public override int DayNumber => 4;

        public Day04(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] input)
        {
            var numbers = GetNumbers(input);
            var boards = GetBoards(input);

            Board? winningBoard = null;
            int index = 0;
            int winningNumber = 0;
            while (winningBoard == null)
            {
                foreach (var board in boards)
                {
                    board.ProcessNumber(numbers[index], out bool wins);
                    if (wins)
                    {
                        winningBoard = board;
                        winningNumber = numbers[index];
                        break;
                    }
                }
                index++;
            }

            return GetResult(winningBoard, winningNumber);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var numbers = GetNumbers(input);
            var boards = GetBoards(input);

            Board? loosingBoard = null;
            int index = 0;
            int loosingNumber = 0;
            while (loosingBoard == null)
            {
                foreach (var board in boards)
                {
                    board.ProcessNumber(numbers[index], out bool wins);
                    if (wins && boards.Count == 1)
                    {
                        loosingBoard = board;
                        loosingNumber = numbers[index];
                        break;
                    }
                }
                index++;
                boards = boards
                    .Where(b => !b.Winning)
                    .ToList();
            }

            return GetResult(loosingBoard, loosingNumber);
        }

        private long GetResult(Board winningBoard, int winningNumber)
        {
            var cellValue = winningBoard.Cells
                .Where(c => !c.Marked)
                .Sum(c => c.Value);

            return cellValue * winningNumber;
        }

        private int[] GetNumbers(string[] input)
        {
            return input[0]
               .Split(NumberSplit)
               .Select(i => int.Parse(i))
               .ToArray();
        }

        private List<Board> GetBoards(string[] input)
        {
            List<Board> result = new();
            Board? currentBoard = null;
            int currentLine = 1;
            int currentRow = 0;
            while (currentLine < input.Length)
            {
                if (string.IsNullOrEmpty(input[currentLine]))
                {
                    currentRow = 0;
                    currentBoard = new Board();
                    result.Add(currentBoard);
                    currentLine++;
                    continue;
                }

                var row = input[currentLine]
                    .Split(ValueSplit, StringSplitOptions.RemoveEmptyEntries)
                    .Select((value, column) => new Cell
                    {
                        Column = column,
                        Row = currentRow,
                        Value = int.Parse(value)
                    });

                currentBoard!.Cells.AddRange(row);

                currentLine++;
                currentRow++;
            }

            return result;
        }


    }
}
