namespace AdventOfCode2023.Days.Day19Group
{
    public record Ranges
    {
        public List<int> X { get; set; } = Enumerable.Range(1, 4000).ToList();
        public List<int> M { get; set; } = Enumerable.Range(1, 4000).ToList();
        public List<int> A { get; set; } = Enumerable.Range(1, 4000).ToList();
        public List<int> S { get; set; } = Enumerable.Range(1, 4000).ToList();

        public Ranges Merge(Ranges b)
        {
            return new Ranges
            {
                X = X
                    .Union(b.X)
                    .ToList(),
                M = M
                    .Union(b.M)
                    .ToList(),
                A = A
                    .Union(b.A)
                    .ToList(),
                S = S
                    .Union(b.S)
                    .ToList(),
            };
        }

        public Ranges Copy()
        {
            return new Ranges
            {
                X = X.ToList(),
                M = M.ToList(),
                A = A.ToList(),
                S = S.ToList()
            };
        }
    }
}
