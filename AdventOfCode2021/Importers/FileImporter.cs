namespace AdventOfCode2021.Importers
{
    public class FileImporter : IFileImporter
    {
        public async Task<string[]> GetInputAsync(int dayNumber)
        {
            if (!File.Exists(@$"{Environment.CurrentDirectory}\Inputs\input{dayNumber:00}.txt"))
            {
                throw new NotImplementedException();
            }

            var lines = await File.ReadAllLinesAsync(@$"{Environment.CurrentDirectory}\Inputs\Input{dayNumber:00}.txt");

            return lines;
        }
    }
}
