namespace SalesProcessor.Application.Ports.Driven
{
    public interface IFileNameService
    {
        string GetStatisticsFileName(string inputFile, string outputPath);
    }
}