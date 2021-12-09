using AdventOfCode2021.Enums;

namespace AdventOfCode2021.Days
{
    public interface IDay
    {
        Task<(long answer, long duration)> ProcessPartAsync(Part part);
    }
}
