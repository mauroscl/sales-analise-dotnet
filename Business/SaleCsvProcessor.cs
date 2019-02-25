using System;
using System.IO;

namespace Business
{
    public class SaleCsvProcessor: ISaleDataProcessor
    {
        private readonly ISalesContextLoader _salesContextLoader;
        private readonly ISalesStatisticsService _salesStatisticsService;
        private readonly ISalesSummaryOutputService _salesSummaryOutputService;

        public SaleCsvProcessor(ISalesContextLoader salesContextLoader, ISalesStatisticsService salesStatisticsService, 
            ISalesSummaryOutputService salesSummaryOutputService)
        {
            _salesContextLoader = salesContextLoader;
            _salesStatisticsService = salesStatisticsService;
            _salesSummaryOutputService = salesSummaryOutputService;
        }

        public void Process(string sourceFile, string destinationPath)
        {
            Console.WriteLine("Processing file: " + sourceFile);
            var salesContext = _salesContextLoader.Load(sourceFile);

            var destinationFileName = Path.GetFileNameWithoutExtension(sourceFile) + ".done" + Path.GetExtension(sourceFile);
            var destinationFileFullPath = Path.Combine(destinationPath, destinationFileName);

            var destinationProcessedFile = Path.Combine(Path.GetDirectoryName(sourceFile), "processed",
                Path.GetFileName(sourceFile));

            var mostExpensiveSales = _salesStatisticsService.CalculateMostExpensiveSales(salesContext.Sales);
            var worstSellers = _salesStatisticsService.CalculateWorstSellers(salesContext.Sales);

            var salesSummary = new SalesSummary(salesContext.AmountSalesman, salesContext.AmountCustomer, worstSellers, mostExpensiveSales);

            _salesSummaryOutputService.Write(destinationFileFullPath, salesSummary);

            File.Move(sourceFile, destinationProcessedFile);

            Console.WriteLine("File Processed: " + sourceFile);
        }
    }
}
