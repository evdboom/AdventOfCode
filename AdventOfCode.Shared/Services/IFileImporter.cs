namespace AdventOfCode.Shared.Services
{   
    public interface IFileImporter
    {
        Task<string[]> GetInputAsync(int dayNumber);
    }
}
