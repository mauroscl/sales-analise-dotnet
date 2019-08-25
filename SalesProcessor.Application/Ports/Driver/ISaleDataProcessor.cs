using System.Collections.Generic;

namespace SalesProcessor.Application.Ports.Driver
{
    public interface ISaleDataProcessor
    {
        void Process(string saleContent, string saleKey);
    }
}