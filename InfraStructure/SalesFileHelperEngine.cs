using System;
using Business;
using FileHelpers;

namespace InfraStructure
{
    public class SalesFileHelperEngine : IFileHelperEngine
    {
        private const string SalesmanIdentifier = "001";
        private const string CustomerIdentifier = "002";
        private const string SaleIdentifier = "003";

        public object[] ReadCsvFile(string filePath)
        {
            var engine = new MultiRecordEngine(typeof(Salesman),
                    typeof(Customer),
                    typeof(Sale))
                { RecordSelector = CustomSelector };

            return engine.ReadFile(filePath);

        }

        private Type CustomSelector(MultiRecordEngine engine, string recordLine)
        {
            if (String.IsNullOrEmpty(recordLine) || recordLine.Length < 3)
                return null;

            var typeIdentifier = recordLine.Substring(0, 3);
            Type type = null;
            switch (typeIdentifier)
            {
                case SalesmanIdentifier:
                    type = typeof(Salesman);
                    break;
                case CustomerIdentifier:
                    type = typeof(Customer);
                    break;
                case SaleIdentifier:
                    type = typeof(Sale);
                    break;
            }

            return type;
        }
    }
}