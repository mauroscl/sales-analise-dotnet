using System;
using Business.Domain;
using Business.Ports;

namespace Business.UseCases
{
    public class SaleCsvProcessor: ISaleDataProcessor
    {
        private readonly ISalesContextLoader _salesContextLoader;
        private readonly ISalesStatisticsService _salesStatisticsService;
        private readonly ISalesSummaryOutputService _salesSummaryOutputService;
        private readonly IFileService _fileService;

        public SaleCsvProcessor(ISalesContextLoader salesContextLoader, ISalesStatisticsService salesStatisticsService, 
            ISalesSummaryOutputService salesSummaryOutputService, IFileService fileService)
        {
            _salesContextLoader = salesContextLoader;
            _salesStatisticsService = salesStatisticsService;
            _salesSummaryOutputService = salesSummaryOutputService;
            _fileService = fileService;
        }

        public void Process(string inputFile, string outputPath)
        {
            Console.WriteLine("Processing file: " + inputFile);

            var salesContext = _salesContextLoader.Load(inputFile);

            var mostExpensiveSales = _salesStatisticsService.CalculateMostExpensiveSales(salesContext.Sales);
            var worstSellers = _salesStatisticsService.CalculateWorstSellers(salesContext.Sales);

            var salesSummary = new SalesSummary(salesContext.AmountSalesman, salesContext.AmountCustomer, worstSellers, mostExpensiveSales);

            var outputFilePath = _fileService.GetStatisticsFileName(inputFile, outputPath);
            _salesSummaryOutputService.Write(outputFilePath, salesSummary);

            _fileService.MoveProcessedFile(inputFile);

            Console.WriteLine("File Processed: " + inputFile);
        }
    }
}
