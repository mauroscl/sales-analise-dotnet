using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IFileService
    {
        IList<string> GetUnprocessedFiles(string inputDirectory);
        void CreateApplicationDirectories(string inputPath, string outputPath);
        void MoveProcessedFile(string inputFile);
        string GetStatisticsFileName(string inputFile, string outputPath);
    }
}
