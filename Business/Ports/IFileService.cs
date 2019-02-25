using System.Collections.Generic;

namespace Business.Ports
{
    public interface IFileService
    {
        IList<string> GetUnprocessedFiles(string inputDirectory);
        void CreateApplicationDirectories(string inputPath, string outputPath);
        void MoveProcessedFile(string inputFile);
        string GetStatisticsFileName(string inputFile, string outputPath);
    }
}
