using System;
using System.Collections.Generic;

namespace Business
{
    public interface ISalesStatisticsService
    {
        IList<String> CalculateWorstSellers(IList<Sale> sales);
        IList<String> CalculateMostExpensiveSales(IList<Sale> sales);
    }
}