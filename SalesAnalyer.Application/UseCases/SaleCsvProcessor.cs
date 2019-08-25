using System;
using SalesAnalyzer.Application.Domain;
using SalesAnalyzer.Application.Ports;

namespace SalesAnalyzer.Application.UseCases
{
    public class SaleCsvProcessor : ISaleDataProcessor
    {
        private readonly ISalesContextLoader _salesContextLoader;
        private readonly ISalesStatisticsService _salesStatisticsService;

        public SaleCsvProcessor(ISalesContextLoader salesContextLoader, ISalesStatisticsService salesStatisticsService)
        {
            _salesContextLoader = salesContextLoader;
            _salesStatisticsService = salesStatisticsService;
        }

        public SalesSummary Process(string inputFile, string outputPath)
        {
            Console.WriteLine("Processing file: " + inputFile);

            //var salesContext = _salesContextLoader.Load(inputFile);
            var salesContext = _salesContextLoader.LoadCsv(inputFile);

            var mostExpensiveSales = _salesStatisticsService.CalculateMostExpensiveSales(salesContext.Sales);
            var worstSellers = _salesStatisticsService.CalculateWorstSellers(salesContext.Sales);

            return new SalesSummary(salesContext.AmountSalesman, salesContext.AmountCustomer, worstSellers,
                mostExpensiveSales);
        }
    }
}