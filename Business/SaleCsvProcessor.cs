using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    class SaleCsvProcessor: ISaleDataProcessor
    {
        private ISalesContextLoader _salesContextLoader;
        private ISalesStatisticsService _salesStatisticsService;

        public SaleCsvProcessor()
        {
            //this.salesContextLoader = new SalesContextLoader();
            this._salesStatisticsService = new SalesStatisticsService();
        }

        public void Process(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
