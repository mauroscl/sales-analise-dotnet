using Business.Domain;

namespace Business.Ports
{
    public interface ISalesContextLoader
    {
        SalesContext Load(string filePath);
        SalesContext LoadCsv(string csvContent);
    }
}