using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace Infra
{
    [DelimitedRecord("ç")]
    class Salesman
    {
        private string RowIdentifier { get; set; }
        public string Cpf { get; protected set; }
        public string Name { get; protected set; }
        public double Salary { get; protected set; }
    }
}
