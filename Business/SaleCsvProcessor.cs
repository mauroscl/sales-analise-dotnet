using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    class SaleCsvProcessor: ISaleDataProcessor
    {
        private ISalesContextLoader salesContextLoader;
        private ISalesStatisticsService salesStatisticsService;

        public SaleCsvProcessor()
        {
            this.salesContextLoader = new SalesContextLoader();
            this.salesStatisticsService = new SalesStatisticsService();
        }

        public void Process(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
