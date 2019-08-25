using SalesProcessor.Application.Ports.Driven;
using SalesProcessor.Application.Ports.Driver;
using System.Collections.Generic;

namespace SalesProcessor.Application.UseCases
{
    public class SaleInputProcessor: ISaleInputProcessor
    {

        private readonly ISalesAnalyserIntegrator _integrator;

        public SaleInputProcessor(ISalesAnalyserIntegrator integrator)
        {
            _integrator = integrator;
        }


        public void Process(string saleContent, IReadOnlyDictionary<string, string> headers)
        {
            _integrator.SendSaleData(saleContent, headers);
        }
    }
}
