namespace AdventOfCode2021.Constructs.Day12
{
    public record Cave
    {
        public string Code { get; init; }
        public bool LargeCave { get; init; }
        public IDictionary<string, Cave> Connections { get; set; }

        public Cave(string code)
        {
            Code = code;
            LargeCave = Code.ToUpper().Equals(Code);
            Connections = new Dictionary<string, Cave>();
        }
    }
}
