using System.Collections.Generic;

namespace Business.Domain
{
    public interface ISalesStatisticsService
    {
        IList<string> CalculateWorstSellers(IList<Sale> sales);
        IList<string> CalculateMostExpensiveSales(IList<Sale> sales);
    }
}