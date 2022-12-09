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

        protected override string[] Day06()
        {
            return _testDataPart switch
            {
                "A" => new[] { "mjqjpqmgbljsphdztnvjfqwrcgsmlb" },
                "B" => new[] { "bvwbjplbgvbhsrlpgdmjqwftvncz" },
                "C" => new[] { "nppdvjthqldpwncqszvftbrmjlhg" },
                "D" => new[] { "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg" },
                "E" => new[] { "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw" },
                _ => throw new InvalidOperationException("Not a valid testdata")
            };
        }

        protected override string[] Day07()
        {
            return new[]
            {
                "$ cd /",
                "$ ls",
                "dir a",
                "14848514 b.txt",
                "8504156 c.dat",
                "dir d",
                "$ cd a",
                "$ ls",
                "dir e",
                "29116 f",
                "2557 g",
                "62596 h.lst",
                "$ cd e",
                "$ ls",
                "584 i",
                "$ cd ..",
                "$ cd ..",
                "$ cd d",
                "$ ls",
                "4060174 j",
                "8033020 d.log",
                "5626152 d.ext",
                "7214296 k",
            };
        }

        protected override string[] Day08()
        {
            return new[]
            {
                "30373",
                "25512",
                "65332",
                "33549",
                "35390",
            };
        }

        protected override string[] Day09()
        {
            if (_testDataPart == "A")
            {
                return new[]
                {
                    "R 5",
                    "U 8",
                    "L 8",
                    "D 3",
                    "R 17",
                    "D 10",
                    "L 25",
                    "U 20",
                };
            }

            return new[]
            {
                "R 4",
                "U 4",
                "L 3",
                "D 1",
                "R 4",
                "D 1",
                "L 5",
                "R 2",
            };
        }
    }
}
