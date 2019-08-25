namespace InfraStructure
{
    public interface ISalesSummaryOutputService
    {
        void Write(string path, SalesSummary salesSummary);
    }
}