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

        protected override string[] Day10()
        {
            return new[]
            {
                "addx 15",
                "addx -11",
                "addx 6",
                "addx -3",
                "addx 5",
                "addx -1",
                "addx -8",
                "addx 13",
                "addx 4",
                "noop",
                "addx -1",
                "addx 5",
                "addx -1",
                "addx 5",
                "addx -1",
                "addx 5",
                "addx -1",
                "addx 5",
                "addx -1",
                "addx -35",
                "addx 1",
                "addx 24",
                "addx -19",
                "addx 1",
                "addx 16",
                "addx -11",
                "noop",
                "noop",
                "addx 21",
                "addx -15",
                "noop",
                "noop",
                "addx -3",
                "addx 9",
                "addx 1",
                "addx -3",
                "addx 8",
                "addx 1",
                "addx 5",
                "noop",
                "noop",
                "noop",
                "noop",
                "noop",
                "addx -36",
                "noop",
                "addx 1",
                "addx 7",
                "noop",
                "noop",
                "noop",
                "addx 2",
                "addx 6",
                "noop",
                "noop",
                "noop",
                "noop",
                "noop",
                "addx 1",
                "noop",
                "noop",
                "addx 7",
                "addx 1",
                "noop",
                "addx -13",
                "addx 13",
                "addx 7",
                "noop",
                "addx 1",
                "addx -33",
                "noop",
                "noop",
                "noop",
                "addx 2",
                "noop",
                "noop",
                "noop",
                "addx 8",
                "noop",
                "addx -1",
                "addx 2",
                "addx 1",
                "noop",
                "addx 17",
                "addx -9",
                "addx 1",
                "addx 1",
                "addx -3",
                "addx 11",
                "noop",
                "noop",
                "addx 1",
                "noop",
                "addx 1",
                "noop",
                "noop",
                "addx -13",
                "addx -19",
                "addx 1",
                "addx 3",
                "addx 26",
                "addx -30",
                "addx 12",
                "addx -1",
                "addx 3",
                "addx 1",
                "noop",
                "noop",
                "noop",
                "addx -9",
                "addx 18",
                "addx 1",
                "addx 2",
                "noop",
                "noop",
                "addx 9",
                "noop",
                "noop",
                "noop",
                "addx -1",
                "addx 2",
                "addx -37",
                "addx 1",
                "addx 3",
                "noop",
                "addx 15",
                "addx -21",
                "addx 22",
                "addx -6",
                "addx 1",
                "noop",
                "addx 2",
                "addx 1",
                "noop",
                "addx -10",
                "noop",
                "noop",
                "addx 20",
                "addx 1",
                "addx 2",
                "addx 2",
                "addx -6",
                "addx -11",
                "noop",
                "noop",
                "noop",
            };
        }

        protected override string[] Day11()
        {
            return new[]
            {
                "Monkey 0:",
                "  Starting items: 79, 98",
                "  Operation: new = old * 19",
                "  Test: divisible by 23",
                "    If true: throw to monkey 2",
                "    If false: throw to monkey 3",
                "",
                "Monkey 1:",
                "  Starting items: 54, 65, 75, 74",
                "  Operation: new = old + 6",
                "  Test: divisible by 19",
                "    If true: throw to monkey 2",
                "    If false: throw to monkey 0",
                "",
                "Monkey 2:",
                "  Starting items: 79, 60, 97",
                "  Operation: new = old * old",
                "  Test: divisible by 13",
                "    If true: throw to monkey 1",
                "    If false: throw to monkey 3",
                "",
                "Monkey 3:",
                "  Starting items: 74",
                "  Operation: new = old + 3",
                "  Test: divisible by 17",
                "    If true: throw to monkey 0",
                "    If false: throw to monkey 1",
            };
        }

        protected override string[] Day12()
        {
            return new[]
            {
                "Sabqponm",
                "abcryxxl",
                "accszExk",
                "acctuvwj",
                "abdefghi",
            };
        }

        protected override string[] Day13()
        {
            return new[]
            {
                "[1,1,3,1,1]",
                "[1,1,5,1,1]",
                "",
                "[[1],[2,3,4]]",
                "[[1],4]",
                "",
                "[9]",
                "[[8,7,6]]",
                "",
                "[[4,4],4,4]",
                "[[4,4],4,4,4]",
                "",
                "[7,7,7,7]",
                "[7,7,7]",
                "",
                "[]",
                "[3]",
                "",
                "[[[]]]",
                "[[]]",
                "",
                "[1,[2,[3,[4,[5,6,7]]]],8,9]",
                "[1,[2,[3,[4,[5,6,0]]]],8,9]",
            };
        }

        protected override string[] Day14()
        {
            return new[]
            {
                "498,4 -> 498,6 -> 496,6",
                "503,4 -> 502,4 -> 502,9 -> 494,9",
            };
        }

        protected override string[] Day15()
        {
            return new[]
            {
                "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
                "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
                "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
                "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
                "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
                "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
                "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
                "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
                "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
                "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
                "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
                "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
                "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
                "Sensor at x=20, y=1: closest beacon is at x=15, y=3",
            };
        }

        protected override string[] Day16()
        {
            return new[]
            {              
                "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
                "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
                "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
                "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
                "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
                "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
                "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
                "Valve HH has flow rate=22; tunnel leads to valve GG",
                "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
                "Valve JJ has flow rate=21; tunnel leads to valve II",                
            };
        }

        protected override string[] Day17()
        {
            return new[]
            {
                ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
            };
        }

        protected override string[] Day18() 
        {
            return new[]
            {
                "2,2,2",
                "1,2,2",
                "3,2,2",
                "2,1,2",
                "2,3,2",
                "2,2,1",
                "2,2,3",
                "2,2,4",
                "2,2,6",
                "1,2,5",
                "3,2,5",
                "2,1,5",
                "2,3,5",
            };
        }

        protected override string[] Day19()
        {
            return new[]
            {
                "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.",
                "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.",
            };
        }

        protected override string[] Day20()
        {
            return new[]
            {
                "1",
                "2",
                "-3",
                "3",
                "-2",
                "0",
                "4",
            };
        }

        protected override string[] Day21()
        {
            return new[]
            {
                "root: pppw + sjmn",
                "dbpl: 5",
                "cczh: sllz + lgvd",
                "zczc: 2",
                "ptdq: humn - dvpt",
                "dvpt: 3",
                "lfqf: 4",
                "humn: 5",
                "ljgn: 2",
                "sjmn: drzm * dbpl",
                "sllz: 4",
                "pppw: cczh / lfqf",
                "lgvd: ljgn * ptdq",
                "drzm: hmdt - zczc",
                "hmdt: 32",
            };
        }

        protected override string[] Day22()
        {
            return new[]
            {
                "        ...#",
                "        .#..",
                "        #...",
                "        ....",
                "...#.......#",
                "........#...",
                "..#....#....",
                "..........#.",
                "        ...#....",
                "        .....#..",
                "        .#......",
                "        ......#.",
                "",
                "10R5L5R10L4R5L5",
            };
        }

        protected override string[] Day23()
        {
            return new[]
            {
                "....#..",
                "..###.#",
                "#...#.#",
                ".#...##",
                "#.###..",
                "##.#.##",
                ".#..#..",
            };
        }

        protected override string[] Day24()
        {
            return new[]
            {                
                "#.######",
                "#>>.<^<#",
                "#.<..<<#",
                "#>v.><>#",
                "#<^v^^>#",
                "######.#",                
            };
        }

        protected override string[] Day25()
        {
            return new[]
            {
                "1=-0-2",
                "12111",
                "2=0=",
                "21",
                "2=01",
                "111",
                "20012",
                "112",
                "1=-1=",
                "1-12",
                "12",
                "1=",
                "122",
            };
        }
    }
}
