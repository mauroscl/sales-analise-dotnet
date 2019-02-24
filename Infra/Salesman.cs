using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace Infra
{
    [DelimitedRecord("ç")]
    class Salesman
    {
        private String RowIdentifier { get; set; }
        public String Cpf { get; protected set; }
        public String Name { get; protected set; }
        public double Salary { get; protected set; }
    }
}
