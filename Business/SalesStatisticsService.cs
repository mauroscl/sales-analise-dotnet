using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BusinessTest")]

namespace Business
{
    class SalesStatisticsService
    {
        internal IList<String> CalculateWorstSellers(IList<Sale> sales)
        {
            if (!sales.Any())
            {
                return new List<string>();
            }
            var salesBySalesman = sales
                .GroupBy(sale => sale.Salesman)
                .Select(group => new
                {
                    Salesman = group.Key,
                    Total = group.Sum(sale => sale.Total)
                }).ToList();

            double minTotal = salesBySalesman.Min(x => x.Total);

            return salesBySalesman
                .Where(sale => sale.Total.Equals(minTotal))
                .Select(sale => sale.Salesman).ToList();
        }

        internal IEnumerable<String> CalculateMostExpensiveSales(IList<Sale> sales)
        {
            if (!sales.Any())
            {
                return new List<string>();
            }
            double maxTotal = sales.Max(sale => sale.Total);
            return sales
                .Where(sale => sale.Total.Equals(maxTotal))
                .Select(sale => sale.SaleId);
        }
    }
}
