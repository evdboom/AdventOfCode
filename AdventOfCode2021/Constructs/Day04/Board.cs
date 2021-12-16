using System.Text;

namespace AdventOfCode2021.Constructs.Day04
{
    public record Board
    {
        public bool Winning { get; private set; }
        public List<Cell> Cells { get; }

        public Board()
        {
            Cells = new List<Cell>();            
        }

        public void ProcessNumber(int number, out bool winning)
        {
            if (Cells.FirstOrDefault(c => c.Value == number) is Cell cell)
            {
                cell.Marked = true;

                var row = Cells
                    .Where(c => c.Row == cell.Row);
                var column = Cells
                    .Where(c => c.Column == cell.Column);

                Winning = !Winning ? row.All(c => c.Marked) || column.All(c => c.Marked) : Winning;
            }
            
            winning = Winning;
        }
    }
}
