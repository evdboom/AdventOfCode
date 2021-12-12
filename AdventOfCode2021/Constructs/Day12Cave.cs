using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Constructs
{
    public record Day12Cave
    {
        public string Code { get; init; }
        public bool LargeCave { get; init; }
        public IDictionary<string,Day12Cave> Connections { get; set; }

        public Day12Cave(string code)
        {
            Code = code;
            LargeCave = Code.ToUpper().Equals(Code);
            Connections = new Dictionary<string,Day12Cave>();
        }
    }
}
