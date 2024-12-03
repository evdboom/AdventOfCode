namespace AdventOfCode.Shared.Testing
{
    public abstract class TestDataBase<Data> : ITestData<Data>
        where Data : ITestData<Data>, new()
    {
        protected string? _testDataPart;
        public void SetTestDataPart(string testDataPart)
        {
            _testDataPart = testDataPart;
        }

        public Task<string[]> GetInputAsync(int dayNumber)
        {
            var result = dayNumber switch
            {
                1 => Day01(),
                2 => Day02(),
                3 => Day03(),
                4 => Day04(),
                5 => Day05(),
                6 => Day06(),
                7 => Day07(),
                8 => Day08(),
                9 => Day09(),
                10 => Day10(),
                11 => Day11(),
                12 => Day12(),
                13 => Day13(),
                14 => Day14(),
                15 => Day15(),
                16 => Day16(),
                17 => Day17(),
                18 => Day18(),
                19 => Day19(),
                20 => Day20(),
                21 => Day21(),
                22 => Day22(),
                23 => Day23(),
                24 => Day24(),
                25 => Day25(),
                _ => throw new ArgumentException("Unknown day", nameof(dayNumber))
            };

            return Task.FromResult(result);
        }

        protected virtual string[] Day01()
        {
            return [];
        }

        protected virtual string[] Day02()
        {
            return [];
        }

        protected virtual string[] Day03()
        {
            return [];
        }

        protected virtual string[] Day04()
        {
            return [];
        }

        protected virtual string[] Day05()
        {
            return [];
        }

        protected virtual string[] Day06()
        {
            return [];
        }

        protected virtual string[] Day07()
        {
            return [];
        }

        protected virtual string[] Day08()
        {
            return [];
        }

        protected virtual string[] Day09()
        {
            return [];
        }

        protected virtual string[] Day10()
        {
            return [];
        }

        protected virtual string[] Day11()
        {
            return [];
        }

        protected virtual string[] Day12()
        {
            return [];
        }

        protected virtual string[] Day13()
        {
            return [];
        }

        protected virtual string[] Day14()
        {
            return [];
        }

        protected virtual string[] Day15()
        {
            return [];
        }

        protected virtual string[] Day16()
        {
            return [];
        }

        protected virtual string[] Day17()
        {
            return [];
        }

        protected virtual string[] Day18()
        {
            return [];
        }

        protected virtual string[] Day19()
        {
            return [];
        }

        protected virtual string[] Day20()
        {
            return [];
        }

        protected virtual string[] Day21()
        {
            return [];
        }

        protected virtual string[] Day22()
        {
            return [];
        }

        protected virtual string[] Day23()
        {
            return [];
        }

        protected virtual string[] Day24()
        {
            return [];
        }

        protected virtual string[] Day25()
        {
            return [];
        }
    }
}
