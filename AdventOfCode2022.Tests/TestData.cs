using AdventOfCode.Shared.Testing;

namespace AdventOfCode2022.Tests
{
    public class TestData : TestDataBase<TestData>
    {
        protected override string[] Day01()
        {
            return new[]
            {
                "1000",
                "2000",
                "3000",
                "",
                "4000",
                "",
                "5000",
                "6000",
                "",
                "7000",
                "8000",
                "9000",
                "",
                "10000",
            };
        }

        protected override string[] Day02()
        {
            return new[]
            {
                "A Y",
                "B X",
                "C Z"
            };
        }

        protected override string[] Day03()
        {
            return new[]
            {
                "vJrwpWtwJgWrhcsFMMfFFhFp",
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
                "PmmdzqPrVvPwwTWBwg",
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
                "ttgJtRGJQctTZtZT",
                "CrZsJsPPZsGzwwsLwLmpwMDw",
            };
        }

        protected override string[] Day04()
        {
            return new[]
            {
                "2-4,6-8",
                "2-3,4-5",
                "5-7,7-9",
                "2-8,3-7",
                "6-6,4-6",
                "2-6,4-8",
            };
        }

        protected override string[] Day05()
        {
            return new[]
            {
               "    [D]    ",
               "[N] [C]    ",
               "[Z] [M] [P]",
               " 1   2   3 ",
               "",
               "move 1 from 2 to 1",
               "move 3 from 1 to 3",
               "move 2 from 2 to 1",
               "move 1 from 1 to 2"
            };
        }
    }
}
