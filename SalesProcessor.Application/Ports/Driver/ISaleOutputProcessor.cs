using SalesProcessor.Application.UseCases;

namespace SalesProcessor.Application.Ports.Driver
{
    public interface ISaleOutputProcessor
    {
        void PersistStatistics(SalesSummary salesSummary, string fileName, string inputPath, string outputPath);
    }
}