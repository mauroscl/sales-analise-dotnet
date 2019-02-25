using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public void Process(string sourcePath, string destinationPath)
        {
            var salesContext = _salesContextLoader.Load(sourcePath);

            var destinationFileName = Path.GetFileNameWithoutExtension(sourcePath) + ".done" + Path.GetExtension(sourcePath);
            var destinationFileFullPath = Path.Combine(destinationPath, destinationFileName);

            var destinationProcessedFile = Path.Combine(Path.GetDirectoryName(sourcePath), "processed",
                Path.GetFileName(sourcePath));

            var mostExpensiveSales = _salesStatisticsService.CalculateMostExpensiveSales(salesContext.Sales);
            var worstSellers = _salesStatisticsService.CalculateWorstSellers(salesContext.Sales);

            var salesSummary = new SalesSummary(salesContext.AmountSalesman, salesContext.AmountCustomer, worstSellers, mostExpensiveSales);

            _salesSummaryOutputService.Write(destinationFileFullPath, salesSummary);

            File.Move(sourcePath, destinationProcessedFile);




        }
    }
}
