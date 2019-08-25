using System.Collections.Generic;

namespace SalesAnalyzer.Application.Domain
{
    public class SalesContext
    {
        public SalesContext(int amountSalesman, int amountCustomer, IList<Sale> sales)
        {
            AmountSalesman = amountSalesman;
            AmountCustomer = amountCustomer;
            Sales = sales;
        }

        public int AmountSalesman { get; }
        public int AmountCustomer { get; }
        public IList<Sale> Sales { get; }
    }
}