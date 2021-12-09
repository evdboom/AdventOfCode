using System.Text;

namespace AdventOfCode2021.Constructs
{
    public record Day04Board
    {
        public bool Winning { get; private set; }
        public List<Day04Cell> Cells { get; }

        public Day04Board()
        {
            Cells = new List<Day04Cell>();            
        }

        public void ProcessNumber(int number, out bool winning)
        {
            if (Cells.FirstOrDefault(c => c.Value == number) is Day04Cell cell)
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
