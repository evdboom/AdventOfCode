namespace AdventOfCode2021.Services
{
    public class FileImporter : IFileImporter
    {
        public async Task<string[]> GetInputAsync(int dayNumber)
        {
            if (!File.Exists(@$"{Environment.CurrentDirectory}\Inputs\input{dayNumber:00}.txt"))
            {
                throw new FileNotFoundException($"Cannot find input{dayNumber:00}.txt, did you store it in the Inputs folder?");
            }

            var lines = await File.ReadAllLinesAsync(@$"{Environment.CurrentDirectory}\Inputs\Input{dayNumber:00}.txt");

            return lines;
        }
    }
}
