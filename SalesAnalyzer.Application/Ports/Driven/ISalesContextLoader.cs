using SalesAnalyzer.Application.Domain;

namespace SalesAnalyzer.Application.Ports.Driven
{
    public interface ISalesContextLoader
    {
        SalesContext LoadCsv(string csvContent);
    }
}