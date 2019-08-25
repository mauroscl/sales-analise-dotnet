using SalesAnalyzer.Application.Domain;

namespace SalesAnalyzer.Application.Ports.Driver
{
    public interface ISaleDataProcessor
    {
        SalesSummary Process(string inputFile);
    }
}