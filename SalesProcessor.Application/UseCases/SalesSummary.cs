using System.Collections.Generic;

namespace SalesProcessor.Application.UseCases
{
    public class SalesSummary
    {
        public SalesSummary(int amountSalesman, int amountCustomer, IList<string> worstSellers,
            IList<string> mostExpensiveSales)
        {
            AmountSalesman = amountSalesman;
            AmountCustomer = amountCustomer;
            WorstSellers = worstSellers;
            MostExpensiveSales = mostExpensiveSales;
        }

        public int AmountSalesman { get; }
        public int AmountCustomer { get; }

        public IList<string> WorstSellers { get; }
        public IList<string> MostExpensiveSales { get; }
    }
}