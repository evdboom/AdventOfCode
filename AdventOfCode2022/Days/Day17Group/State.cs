namespace AdventOfCode2022.Days.Day17Group
{
    public record State
    {
        public int Rock { get; set; }
        public int Action { get; set; }
        public string C0 { get; set; } = string.Empty;
        public string C1 { get; set; } = string.Empty;
        public string C2 { get; set; } = string.Empty;
        public string C3 { get; set; } = string.Empty;
        public string C4 { get; set; } = string.Empty;
        public string C5 { get; set; } = string.Empty;
        public string C6 { get; set; } = string.Empty;   
        public string GetColumn(int index)
        {
            return index switch
            {
                0 => C0,
                1 => C1,
                2 => C2,
                3 => C3,
                4 => C4,
                5 => C5,
                6 => C6,
                _ => string.Empty
            };
        }
        public void SetColumn(int index, string value) 
        {
            switch(index)
            {
                case 0:
                    C0 = value;
                    break;
                case 1:
                    C1 = value;
                    break;
                case 2:
                    C2 = value;
                  break;
                case 3:
                    C3 = value;
                    break;
                case 4:
                    C4 = value;
                    break;
                case 5:
                    C5 = value;
                    break;
                case 6:
                    C6 = value;
                    break;
            }
        }
        public IEnumerable<(int Index, string Column)> GetColumns()
        {
            for (int i = 0; i < 7; i++)
            {
                yield return (i, GetColumn(i));
            }
        }
        public char PointFromTop(int column, int index)
        {
            index--;
            var value = GetColumn(column);
            if (index < 0)
            {
                return '.';
            }
            else if (index >= value.Length)
            {
                return '#';
            }
            else
            {
                return value[index];
            }
        }
    }
}
