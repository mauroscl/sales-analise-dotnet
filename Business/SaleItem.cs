using FileHelpers;

namespace Business
{
    [DelimitedRecord("-")]
    public class SaleItem
    {
        public SaleItem()
        {
        }
        public SaleItem(string id, double quantity, double price)
        {
            Id = id;
            Quantity = quantity;
            Price = price;
        }

        public string Id {get; protected set; }
        public double Quantity { get; protected set; }
        public double Price { get; protected set; }

        public double Total => Quantity * Price;
    }
}
