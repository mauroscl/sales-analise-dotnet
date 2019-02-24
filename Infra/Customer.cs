using System;
using FileHelpers;

namespace Infra
{
    [DelimitedRecord("ç")]
    class Customer
    {
        private String RowIdentifier { get; set; }
        public String Cnpj { get; protected set; }
        public String Name { get; protected set; }
        public String BusinessArea { get; protected set; }

    }
}
