using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace Infra
{
    [DelimitedRecord("ç")]
    class SaleItems
    {
        private String RowIdentifier { get; set; }
        public String SaleId { get; protected set; }
        public String Itens { get; protected set; }
        public String Salesman { get; protected set; }

    }
}
