namespace AdventOfCode2021.Constructs
{
    public class Day14Pair
    {
        public string Code { get; }
        private string InsertionCode { get; }
        public long Count { get; set; }

        public Day14Pair(string code, string insertionCode) : this(code, insertionCode, 1)
        {

        }

        public Day14Pair(string code, string insertionCode, long count)
        {
            Code = code;
            InsertionCode = insertionCode;
            Count = count;
        }


        public IEnumerable<Day14Pair> ProcessStep(Dictionary<string, string> rules)
        {
            var code1 = $"{Code[0]}{InsertionCode}";
            yield return new Day14Pair(code1, rules[code1]) { Count = Count };
            var code2 = $"{InsertionCode}{Code[1]}";
            yield return new Day14Pair(code2, rules[code2]) { Count = Count };
        }

        public IEnumerable<KeyValuePair<char, long>> GetCharacterCount()
        {
            yield return new KeyValuePair<char, long>(Code[0], Count);
            yield return new KeyValuePair<char, long>(Code[1], Count);
        }
    }
}
