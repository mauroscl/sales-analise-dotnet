using System.Collections.Generic;
using System.Linq;
using FileHelpers;

namespace Business
{

    [DelimitedRecord("ç")]
    public class Sale
    {
        public Sale(string saleId, string salesman)
        {
            SaleId = saleId;
            Salesman = salesman;
            this.Items = new List<SaleItem>();
        }

        protected Sale()
        {
        }

        private string RowIdentifier { get; set; }
        public string SaleId { get; protected set; }
        [FieldConverter(typeof(SaleItemCustomConverter))]
        public IEnumerable<SaleItem> Items { get; protected set; }
        public string Salesman { get; protected set; }

        public void SetItems(IEnumerable<SaleItem> itens)
        {
            this.Items = itens;
        }

        public double Total => this.Items.Sum(item => item.Total);

    }
}
