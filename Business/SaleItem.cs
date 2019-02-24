using System;
using FileHelpers;

namespace Infra
{
    [DelimitedRecord("-")]
    public class SaleItem
    {
        private string RowIdentifier { get; set; }
        public string Id {get; protected set; }
        public double Quantity { get; protected set; }
        public double Price { get; protected set; }

        public double Total => Quantity * Price;
    }
}
