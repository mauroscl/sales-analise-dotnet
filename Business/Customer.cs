using FileHelpers;

namespace Business
{
    [DelimitedRecord("ç")]
    public class Customer
    {
        private string RowIdentifier { get; set; }
        public string Cnpj { get; protected set; }
        public string Name { get; protected set; }
        public string BusinessArea { get; protected set; }

    }
}
