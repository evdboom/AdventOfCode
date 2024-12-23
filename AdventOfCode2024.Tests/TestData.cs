using AdventOfCode.Shared.Testing;

namespace AdventOfCode2024.Tests
{
    public class TestData : TestDataBase<TestData>
    {
        protected override string[] Day01()
        {
            return
            [
                "3   4",
                "4   3",
                "2   5",
                "1   3",
                "3   9",
                "3   3"
            ];
        }

        protected override string[] Day02()
        {
            return
            [
                "7 6 4 2 1",
                "1 2 7 8 9",
                "9 7 6 2 1",
                "1 3 2 4 5",
                "8 6 4 4 1",
                "1 3 6 7 9",
            ];
        }

        override protected string[] Day03()
        {
            return
            [
                "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))",
            ];
        }

        protected override string[] Day04()
        {
            return
            [
                "MMMSXXMASM",
                "MSAMXMSMSA",
                "AMXSXMAAMM",
                "MSAMASMSMX",
                "XMASAMXAMM",
                "XXAMMXXAMA",
                "SMSMSASXSS",
                "SAXAMASAAA",
                "MAMMMXMMMM",
                "MXMXAXMASX",
            ];
        }

        protected override string[] Day05()
        {
            return
            [
                "47|53",
                "97|13",
                "97|61",
                "97|47",
                "75|29",
                "61|13",
                "75|53",
                "29|13",
                "97|29",
                "53|29",
                "61|53",
                "97|53",
                "61|29",
                "47|13",
                "75|47",
                "97|75",
                "47|61",
                "75|61",
                "47|29",
                "75|13",
                "53|13",
                "",
                "75,47,61,53,29",
                "97,61,53,29,13",
                "75,29,13",
                "75,97,47,61,53",
                "61,13,29",
                "97,13,75,29,47",
            ];
        }

        protected override string[] Day06()
        {
            return
            [
                "....#.....",
                ".........#",
                "..........",
                "..#.......",
                ".......#..",
                "..........",
                ".#..^.....",
                "........#.",
                "#.........",
                "......#...",
            ];           
        }

        protected override string[] Day07()
        {
            return
            [
                "190: 10 19",
                "3267: 81 40 27",
                "83: 17 5",
                "156: 15 6",
                "7290: 6 8 6 15",
                "161011: 16 10 13",
                "192: 17 8 14",
                "21037: 9 7 18 13",
                "292: 11 6 16 20",
            ];
        }

        protected override string[] Day08()
        {
            return            
            [
                "............",
                "........0...",
                ".....0......",
                ".......0....",
                "....0.......",
                "......A.....",
                "............",
                "............",
                "........A...",
                ".........A..",
                "............",
                "............",
            ];            
        }

        protected override string[] Day09()
        {
            return            
            [
                "2333133121414131402",
            ];            
        }

        protected override string[] Day10()
        {
            return            
            [
                "89010123",
                "78121874",
                "87430965",
                "96549874",
                "45678903",
                "32019012",
                "01329801",
                "10456732",                
            ];            
        }

        protected override string[] Day11()
        {
            return            
            [
                "125 17",
            ];            
        }

        protected override string[] Day12()
        {
            return               
            [
                "RRRRIICCFF",
                "RRRRIICCCF",
                "VVRRRCCFFF",
                "VVRCCCJFFF",
                "VVVVCJJCFE",
                "VVIVCCJJEE",
                "VVIIICJJEE",
                "MIIIIIJJEE",
                "MIIISIJEEE",
                "MMMISSJEEE"
            ];                
        }

        protected override string[] Day13()
        {
            return            
            [
                "Button A: X+94, Y+34",
                "Button B: X+22, Y+67",
                "Prize: X=8400, Y=5400",
                "",
                "Button A: X+26, Y+66",
                "Button B: X+67, Y+21",
                "Prize: X=12748, Y=12176",
                "",
                "Button A: X+17, Y+86",
                "Button B: X+84, Y+37",
                "Prize: X=7870, Y=6450",
                "",
                "Button A: X+69, Y+23",
                "Button B: X+27, Y+71",
                "Prize: X=18641, Y=10279",
            ];
        }

        protected override string[] Day14()
        {
            return                
                [
                    "p=0,4 v=3,-3",
                    "p=6,3 v=-1,-3",
                    "p=10,3 v=-1,2",
                    "p=2,0 v=2,-1",
                    "p=0,0 v=1,3",
                    "p=3,0 v=-2,-2",
                    "p=7,6 v=-1,-3",
                    "p=3,0 v=-1,-2",
                    "p=9,3 v=2,3",
                    "p=7,3 v=-1,2",
                    "p=2,4 v=2,-3",
                    "p=9,5 v=-3,-3",
                ];                
        }

        protected override string[] Day15()
        {
            return                
                [
                    "##########",
                    "#..O..O.O#",
                    "#......O.#",
                    "#.OO..O.O#",
                    "#..O@..O.#",
                    "#O#..O...#",
                    "#O..O..O.#",
                    "#.OO.O.OO#",
                    "#....O...#",
                    "##########",
                    "",
                    "<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^",
                    "vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v",
                    "><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<",
                    "<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^",
                    "^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><",
                    "^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^",
                    ">^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^",
                    "<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>",
                    "^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>",
                    "v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^",
                ];                
        }

        protected override string[] Day16()
        {
            return                
                [
                    "#################",
                    "#...#...#...#..E#",
                    "#.#.#.#.#.#.#.#.#",
                    "#.#.#.#...#...#.#",
                    "#.#.#.#.###.#.#.#",
                    "#...#.#.#.....#.#",
                    "#.#.#.#.#.#####.#",
                    "#.#...#.#.#.....#",
                    "#.#.#####.#.###.#",
                    "#.#.#.......#...#",
                    "#.#.###.#####.###",
                    "#.#.#...#.....#.#",
                    "#.#.#.#####.###.#",
                    "#.#.#.........#.#",
                    "#.#.#.#########.#",
                    "#S#.............#",
                    "#################",
                ];                
        }

        protected override string[] Day17()
        {
            return _testDataPart switch
            {
                "PartOne" =>
                [
                    "Register A: 729",
                    "Register B: 0",
                    "Register C: 0",
                    "",
                    "Program: 0,1,5,4,3,0",
                ],
                _ =>
                [
                    "Register A: 2024",
                    "Register B: 0",
                    "Register C: 0",
                    "",
                    "Program: 0,3,5,4,3,0",
                ]
            };
        }

        protected override string[] Day18()
        {
            return
            [               
                "5,4",
                "4,2",
                "4,5",
                "3,0",
                "2,1",
                "6,3",
                "2,4",
                "1,5",
                "0,6",
                "3,3",
                "2,6",
                "5,1",
                "1,2",
                "5,5",
                "2,5",
                "6,5",
                "1,4",
                "0,4",
                "6,4",
                "1,1",
                "6,1",
                "1,0",
                "0,5",
                "1,6",
                "2,0",                
            ];
        }

        protected override string[] Day19()
        {
            return
            [                
                "r, wr, b, g, bwu, rb, gb, br",
                "",
                "brwrr",
                "bggr",
                "gbbr",
                "rrbgbr",
                "ubwu",
                "bwurrg",
                "brgr",
                "bbrgwb",                
            ];
        }

        protected override string[] Day20()
        {
            return
                """"
                ###############
                #...#...#.....#
                #.#.#.#.#.###.#
                #S#...#.#.#...#
                #######.#.#.###
                #######.#.#...#
                #######.#.###.#
                ###..E#...#...#
                ###.#######.###
                #...###...#...#
                #.#####.#.###.#
                #.#...#.#.#...#
                #.#.#.#.#.#.###
                #...#...#...###
                ###############
                """".Split(Environment.NewLine);
        }
    }
}
