using SalesProcessor.Application.Ports.Driven;
using SalesProcessor.Application.Ports.Driver;
using System.Collections.Generic;

namespace SalesProcessor.Application.UseCases
{
    public class SaleDataProcessor: ISaleDataProcessor
    {

        private readonly ISalesAnalyserIntegrator _integrator;

        public SaleDataProcessor(ISalesAnalyserIntegrator integrator)
        {
            _integrator = integrator;
        }


        public void Process(string saleContent, string saleKey)
        {
            _integrator.SendSaleData(saleContent, saleKey);
        }
    }
}
