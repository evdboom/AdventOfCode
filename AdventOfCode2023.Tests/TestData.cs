using AdventOfCode.Shared.Testing;

namespace AdventOfCode2023.Tests
{
    public class TestData : TestDataBase<TestData>
    {
        protected override string[] Day01()
        {

            return _testDataPart switch
            {
                "Part1" => new[]
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


    }
}
