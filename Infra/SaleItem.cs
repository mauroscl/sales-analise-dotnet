using System;
using FileHelpers;

namespace Infra
{
    [DelimitedRecord("-")]
    public class SaleItem
    {
        private String RowIdentifier { get; set; }
        public String Id {get; protected set; }
        public double Quantity { get; protected set; }
        public double Price { get; protected set; }
    }
}
