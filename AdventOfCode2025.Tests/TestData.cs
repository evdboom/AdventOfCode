using AdventOfCode.Shared.Testing;

namespace AdventOfCode2025.Tests;

public class TestData : TestDataBase<TestData>
{
    protected override string[] Day01()
    {
        return ["L68", "L30", "R48", "L5", "R60", "L55", "L1", "L99", "R14", "L82"];
    }

    protected override string[] Day02()
    {
        return
        [
            "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124",
        ];
    }

    protected override string[] Day03()
    {
        return ["987654321111111", "811111111111119", "234234234234278", "818181911112111"];
    }

    protected override string[] Day04()
    {
        return
        [
            "..@@.@@@@.",
            "@@@.@.@.@@",
            "@@@@@.@.@@",
            "@.@@@@..@.",
            "@@.@@@@.@@",
            ".@@@@@@@.@",
            ".@.@.@.@@@",
            "@.@@@.@@@@",
            ".@@@@@@@@.",
            "@.@.@@@.@.",
        ];
    }
}
