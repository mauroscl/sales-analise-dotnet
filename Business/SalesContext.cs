using System.Collections.Generic;

namespace Business
{
    public class SalesContext
    {
        public SalesContext(int amountSalesman, int amountCustomer, IEnumerable<Sale> sales)
        {
            AmountSalesman = amountSalesman;
            AmountCustomer = amountCustomer;
            Sales = sales;
        }

        private int AmountSalesman { get; }
        private int AmountCustomer { get; }
        public IEnumerable<Sale> Sales { get; }
    }
}
