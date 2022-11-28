using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Shared.Days
{
    public interface IDay
    {
        int DayNumber { get; }
        Task<(long answer, long duration)> ProcessPartAsync(Part part);
    }
}
