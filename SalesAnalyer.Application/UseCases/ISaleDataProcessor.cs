using SalesAnalyzer.Application.Domain;

namespace SalesAnalyzer.Application.UseCases
{
    public interface ISaleDataProcessor
    {
        SalesSummary Process(string inputFile, string outputPath);
    }
}