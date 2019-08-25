using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BusinessTest")]

namespace SalesAnalyzer.Application.Domain
{
    public class SalesStatisticsService : ISalesStatisticsService
    {
        public IList<string> CalculateWorstSellers(IList<Sale> sales)
        {
            if (!sales.Any()) return new List<string>();
            var salesBySalesman = sales
                .GroupBy(sale => sale.Salesman)
                .Select(group => new
                {
                    Salesman = group.Key,
                    Total = group.Sum(sale => sale.Total)
                }).ToList();

            var minTotal = salesBySalesman.Min(x => x.Total);

            return salesBySalesman
                .Where(sale => sale.Total.Equals(minTotal))
                .Select(sale => sale.Salesman).ToList();
        }

        public IList<string> CalculateMostExpensiveSales(IList<Sale> sales)
        {
            if (!sales.Any()) return new List<string>();
            var maxTotal = sales.Max(sale => sale.Total);
            return sales
                .Where(sale => sale.Total.Equals(maxTotal))
                .Select(sale => sale.SaleId).ToList();
        }
    }
}