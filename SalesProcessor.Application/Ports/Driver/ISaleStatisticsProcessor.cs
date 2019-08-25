using SalesProcessor.Application.UseCases;

namespace SalesProcessor.Application.Ports.Driver
{
    public interface ISaleStatisticsProcessor
    {
        void PersistStatistics(SalesSummary salesSummary, string fileName, string inputPath, string outputPath);
    }
}