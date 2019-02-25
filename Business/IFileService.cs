using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IFileService
    {
        IList<string> GetUnprocessedFiles(string inputDirectory);
        void CreateApplicationDirectories(string inputPath, string outputPath);
    }
}
