using Business.Domain;

namespace Business.UseCases
{
    public interface ISaleDataProcessor
    {
        SalesSummary Process(string inputFile, string outputPath);
    }
}