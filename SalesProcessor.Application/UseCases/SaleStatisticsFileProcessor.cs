using System.IO;
using InfraStructure;
using SalesProcessor.Application.Ports.Driven;
using SalesProcessor.Application.Ports.Driver;

namespace SalesProcessor.Application.UseCases
{
    public class SaleStatisticsFileProcessor : ISaleStatisticsProcessor
    {

        private readonly IFileService _fileService;
        private readonly IFileNameService _fileNameService;
        private readonly ISalesSummaryOutputService _salesSummaryOutputService;

        public SaleStatisticsFileProcessor(IFileService fileService, ISalesSummaryOutputService salesSummaryOutputService
            , IFileNameService fileNameService)
        {
            _fileService = fileService;
            _salesSummaryOutputService = salesSummaryOutputService;
            _fileNameService = fileNameService;
        }

        public void PersistStatistics(SalesSummary salesSummary, string fileName, string inputPath, string outputPath)
        {
            var outputFilePath = _fileNameService.GetStatisticsFileName(fileName, outputPath);
            _salesSummaryOutputService.Write(outputFilePath, salesSummary);

            var inputFileFullPath = Path.Combine(inputPath, fileName);
            _fileService.MoveProcessedFile(inputFileFullPath);
        }
    }
}
