using System.Collections.Generic;

namespace SalesProcessor.Application.Ports.Driven
{
    public interface IFileService
    {
        IList<string> GetUnprocessedFiles(string inputDirectory);
        void CreateApplicationDirectories(string inputPath, string outputPath);
        void MoveProcessedFile(string inputFileFullPath);
        string GetStatisticsFileName(string inputFile, string outputPath);

    }
}