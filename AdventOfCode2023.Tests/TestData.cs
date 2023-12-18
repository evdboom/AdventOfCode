using AdventOfCode.Shared.Testing;

namespace AdventOfCode2023.Tests
{
    public class TestData : TestDataBase<TestData>
    {
        protected override string[] Day01()
        {

            return _testDataPart switch
            {
                "Part01" => new[]
                {
                    "1abc2",
                    "pqr3stu8vwx",
                    "a1b2c3d4e5f",
                    "treb7uchet"
                },
                _ => new[]
                {
                    "two1nine",
                    "eightwothree",
                    "abcone2threexyz",
                    "xtwone3four",
                    "4nineeightseven2",
                    "zoneight234",
                    "7pqrstsixteen",                    
                }
            };
        }

        protected override string[] Day02()
        {
            return
            [
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
            ];
        }

        protected override string[] Day03()
        {
            return
            [                
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598.."
            ];
        }

        protected override string[] Day04()
        {
            return
            [                
                "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
                "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
                "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
                "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
                "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
                "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"             
            ];
        }

        protected override string[] Day05()
        {
            return
            [                
                "seeds: 79 14 55 13",
                "",
                "seed-to-soil map:",
                "50 98 2",
                "52 50 48",
                "",
                "soil-to-fertilizer map:",
                "0 15 37",
                "37 52 2",
                "39 0 15",
                "",
                "fertilizer-to-water map:",
                "49 53 8",
                "0 11 42",
                "42 0 7",
                "57 7 4",
                "",
                "water-to-light map:",
                "88 18 7",
                "18 25 70",
                "",
                "light-to-temperature map:",
                "45 77 23",
                "81 45 19",
                "68 64 13",
                "",
                "temperature-to-humidity map:",
                "0 69 1",
                "1 0 69",
                "",
                "humidity-to-location map:",
                "60 56 37",
                "56 93 4"
            ];
        }

        protected override string[] Day06()
        {
            return
            [                
                "Time:      7  15   30",
                "Distance:  9  40  200"                
            ];
        }

        protected override string[] Day07()
        {
            return
            [
                "32T3K 765",
                "T55J5 684",
                "KK677 28",
                "KTJJT 220",
                "QQQJA 483"
            ];
        }

        protected override string[] Day08()
        {
            return _testDataPart switch
            {
                "Part01" =>
                [
                    "LLR",
                    "",
                    "AAA = (BBB, BBB)",
                    "BBB = (AAA, ZZZ)",
                    "ZZZ = (ZZZ, ZZZ)",
                ],
                _ =>
                [
                "LR",
                "",
                "11A = (11B, XXX)",
                "11B = (XXX, 11Z)",
                "11Z = (11B, XXX)",
                "22A = (22B, XXX)",
                "22B = (22C, 22C)",
                "22C = (22Z, 22Z)",
                "22Z = (22B, 22B)",
                "XXX = (XXX, XXX)",
                ]
            };                                          
        }

        protected override string[] Day09()
        {
            return
            [                
                "0 3 6 9 12 15",
                "1 3 6 10 15 21",
                "10 13 16 21 30 45",                
            ];
        }

        protected override string[] Day10()
        {
            return _testDataPart switch
            {
                "Part01" =>
                [
                    "7-F7-",
                    ".FJ|7",
                    "SJLL7",
                    "|F--J",
                    "LJ.LJ",
                ],
                _ =>
                [
                    "FF7FSF7F7F7F7F7F---7",
                    "L|LJ||||||||||||F--J",
                    "FL-7LJLJ||||||LJL-77",
                    "F--JF--7||LJLJ7F7FJ-",
                    "L---JF-JLJ.||-FJLJJ7",
                    "|F|F-JF---7F7-L7L|7|",
                    "|FFJF7L7F-JF7|JL---7",
                    "7-L-JL7||F7|L7F-7F7|",
                    "L.L7LFJ|||||FJL7||LJ",
                    "L7JLJL-JLJLJL--JLJ.L",                
                ]
            };
        }

        protected override string[] Day11()
        {
            return
            [
                "...#......",
                ".......#..",
                "#.........",
                "..........",
                "......#...",
                ".#........",
                ".........#",
                "..........",
                ".......#..",
                "#...#.....",
            ];
        }

        protected override string[] Day12()
        {
            return
            [                
                "???.### 1,1,3",
                ".??..??...?##. 1,1,3",
                "?#?#?#?#?#?#?#? 1,3,1,6",
                "????.#...#... 4,1,1",
                "????.######..#####. 1,6,5",
                "?###???????? 3,2,1"
            ];
        }

        protected override string[] Day13()
        {
            return
            [                
                "#.##..##.",
                "..#.##.#.",
                "##......#",
                "##......#",
                "..#.##.#.",
                "..##..##.",
                "#.#.##.#.",
                "",
                "#...##..#",
                "#....#..#",
                "..##..###",
                "#####.##.",
                "#####.##.",
                "..##..###",
                "#....#..#",
            ];
        }

        protected override string[] Day14()
        {
            return
            [                
                "O....#....",
                "O.OO#....#",
                ".....##...",
                "OO.#O....O",
                ".O.....O#.",
                "O.#..O.#.#",
                "..O..#O..O",
                ".......O..",
                "#....###..",
                "#OO..#....",             
            ];
        }
        protected override string[] Day15()
        {
            return ["rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7"];
        }

        protected override string[] Day16()
        {
            return
            [                
                @".|...\....",
                @"|.-.\.....",
                @".....|-...",
                @"........|.",
                @"..........",
                @".........\",
                @"..../.\\..",
                @".-.-/..|..",
                @".|....-|.\",
                @"..//.|....",             
            ];
        }

        protected override string[] Day17()
        {
            return _testDataPart switch
            {
                "first" =>
                [
                    "2413432311323",
                    "3215453535623",
                    "3255245654254",
                    "3446585845452",
                    "4546657867536",
                    "1438598798454",
                    "4457876987766",
                    "3637877979653",
                    "4654967986887",
                    "4564679986453",
                    "1224686865563",
                    "2546548887735",
                    "4322674655533",
                ],
                _ =>
                [                    
                    "111111111111",
                    "999999999991",
                    "999999999991",
                    "999999999991",
                    "999999999991",                   
                ]
            };
        }

        protected override string[] Day18()
        {
            return
            [                
                "R 6 (#70c710)",
                "D 5 (#0dc571)",
                "L 2 (#5713f0)",
                "D 2 (#d2c081)",
                "R 2 (#59c680)",
                "D 2 (#411b91)",
                "L 5 (#8ceee2)",
                "U 2 (#caa173)",
                "L 1 (#1b58a2)",
                "U 2 (#caa171)",
                "R 2 (#7807d2)",
                "U 3 (#a77fa3)",
                "L 2 (#015232)",
                "U 2 (#7a21e3)",                
            ];
        }
    }
}
