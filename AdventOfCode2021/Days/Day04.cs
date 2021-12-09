using AdventOfCode2021.Constructs;
using AdventOfCode2021.Importers;

namespace AdventOfCode2021.Days
{
    public class Day04 : Day
    {
        private const char NumberSplit = ',';
        private const char ValueSplit = ' ';
        protected override int DayNumber => 4;

        public Day04(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] input)
        {
            var numbers = GetNumbers(input);
            var boards = GetBoards(input);

            Day04Board? winningBoard = null;
            int index = 0;
            int winningNumber = 0;
            while (winningBoard == null)
            {
                foreach(var board in boards)
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

            Day04Board? loosingBoard = null;
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

        private long GetResult(Day04Board winningBoard, int winningNumber)
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

        private List<Day04Board> GetBoards(string[] input)
        {
            List<Day04Board> result = new();
            Day04Board? currentBoard = null;
            int currentLine = 1;
            int currentRow = 0;
            while(currentLine < input.Length)
            {                                
                if (string.IsNullOrEmpty(input[currentLine]))
                {
                    currentRow = 0;
                    currentBoard = new Day04Board();
                    result.Add(currentBoard);
                    currentLine++;
                    continue;
                }

                var row = input[currentLine]
                    .Split(ValueSplit, StringSplitOptions.RemoveEmptyEntries)
                    .Select((value, column) => new Day04Cell
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
