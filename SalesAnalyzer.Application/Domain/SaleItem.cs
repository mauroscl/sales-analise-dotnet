using FileHelpers;

namespace SalesAnalyzer.Application.Domain
{
    [DelimitedRecord("-")]
    public class SaleItem
    {
        public SaleItem(string id, double quantity, double price)
        {
            Id = id;
            Quantity = quantity;
            Price = price;
        }

        private SaleItem()
        {
        }

        public string Id { get; protected set; }
        public double Quantity { get; protected set; }
        public double Price { get; protected set; }

        public double Total => Quantity * Price;
    }
}