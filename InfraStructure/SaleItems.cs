using FileHelpers;
using System;

namespace Infra
{
    [DelimitedRecord("ç")]
    public class SaleItems
    {
        private string RowIdentifier { get; set; }
        public string SaleId { get; protected set; }
        public string Itens { get; protected set; }
        public string Salesman { get; protected set; }

    }
}
