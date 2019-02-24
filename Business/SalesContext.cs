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
        private IEnumerable<Sale> Sales { get; }
    }
}
