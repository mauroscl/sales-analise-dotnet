using FileHelpers;

namespace Business.Domain
{
    [DelimitedRecord("ç")]
    public class Customer
    {
        private string RowIdentifier { get; set; }
        public string Cnpj { get; protected internal set; }
        public string Name { get; protected internal set; }
        public string BusinessArea { get; protected internal set; }

    }
}
