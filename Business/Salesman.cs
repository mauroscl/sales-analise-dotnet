using FileHelpers;

namespace Business
{
    [DelimitedRecord("ç")]
    public class Salesman
    {
        private string RowIdentifier { get; set; }
        public string Cpf { get; protected set; }
        public string Name { get; protected set; }
        public double Salary { get; protected set; }
    }
}
