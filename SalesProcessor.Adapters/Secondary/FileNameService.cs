using SalesProcessor.Application.Ports.Driven;
using System.IO;

namespace SalesProcessor.Adapters.Secondary
{
    public class FileNameService: IFileNameService
    {
        public string GetStatisticsFileName(string inputFile, string outputPath)
        {
            var destinationFileName =
                Path.GetFileNameWithoutExtension(inputFile) + ".done" + Path.GetExtension(inputFile);
            return Path.Combine(outputPath, destinationFileName);
        }


    }
}
