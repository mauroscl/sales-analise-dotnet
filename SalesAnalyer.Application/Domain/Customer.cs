using FileHelpers;

namespace SalesAnalyzer.Application.Domain
{
    [DelimitedRecord("ç")]
    public class Customer
    {
        private string RowIdentifier { get; set; }
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string BusinessArea { get; set; }
    }
}