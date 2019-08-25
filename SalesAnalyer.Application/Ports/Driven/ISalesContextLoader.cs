using SalesAnalyzer.Application.Domain;

namespace SalesAnalyzer.Application.Ports.Driven
{
    public interface ISalesContextLoader
    {
        SalesContext Load(string filePath);
        SalesContext LoadCsv(string csvContent);
    }
}