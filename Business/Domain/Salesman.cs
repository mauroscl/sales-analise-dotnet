using System.Runtime.CompilerServices;
using FileHelpers;

[assembly: InternalsVisibleTo("InfraStructureTest")]

namespace Business.Domain
{
    [DelimitedRecord("ç")]
    public class Salesman
    {
        private string RowIdentifier { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
    }
}