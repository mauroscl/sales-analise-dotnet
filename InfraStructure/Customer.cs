using System;
using FileHelpers;

namespace Infra
{
    [DelimitedRecord("ç")]
    class Customer
    {
        private string RowIdentifier { get; set; }
        public string Cnpj { get; protected set; }
        public string Name { get; protected set; }
        public string BusinessArea { get; protected set; }

    }
}
