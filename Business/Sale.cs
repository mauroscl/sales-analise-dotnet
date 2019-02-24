using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class Sale
    {
        public Sale(string saleId, string salesman)
        {
            SaleId = saleId;
            Salesman = salesman;
            this.Items = new List<SaleItem>();
        }

        public string SaleId { get; protected set; }
        public string Salesman { get; protected set; }
        public IEnumerable<SaleItem> Items { get; protected set; }

        public void SetItems(IEnumerable<SaleItem> itens)
        {
            this.Items = itens;
        }

        public double Total => this.Items.Sum(item => item.Total);

    }
}
