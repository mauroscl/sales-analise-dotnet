using Business.Domain;

namespace Business.Ports
{
    public interface ISalesSummaryOutputService
    {
        void Write(string path, SalesSummary salesSummary);
    }

}