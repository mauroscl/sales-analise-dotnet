using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using SalesAnalyzer.Application.Converters;

namespace SalesAnalyzer.Application.Domain
{
    [DelimitedRecord("ç")]
    public class Sale
    {
        public Sale(string saleId, string salesman)
        {
            SaleId = saleId;
            Salesman = salesman;
            Items = new List<SaleItem>();
        }

        private Sale()
        {
        }

        private string RowIdentifier { get; set; }
        public string SaleId { get; protected set; }

        [FieldConverter(typeof(SaleItemCustomConverter))]
        public IEnumerable<SaleItem> Items { get; protected set; }

        public string Salesman { get; protected set; }

        public double Total => Items.Sum(item => item.Total);

        public void SetItems(IEnumerable<SaleItem> itens)
        {
            Items = itens;
        }
    }
}