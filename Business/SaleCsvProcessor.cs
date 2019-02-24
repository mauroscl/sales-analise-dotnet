using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class SaleCsvProcessor: ISaleDataProcessor
    {
        private readonly ISalesContextLoader _salesContextLoader;
        private readonly ISalesStatisticsService _salesStatisticsService;

        public SaleCsvProcessor(ISalesContextLoader salesContextLoader, ISalesStatisticsService salesStatisticsService)
        {
            _salesContextLoader = salesContextLoader;
            _salesStatisticsService = salesStatisticsService;
        }

        public void Process(string filePath)
        {
            var salesContext = _salesContextLoader.Load(filePath);
            var mostExpensiveSales = _salesStatisticsService.CalculateMostExpensiveSales(salesContext.Sales.ToList());
            Console.WriteLine("most expensive: " + String.Join(',', mostExpensiveSales));
        }
    }
}
