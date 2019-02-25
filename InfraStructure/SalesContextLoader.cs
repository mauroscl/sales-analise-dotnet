using Business;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraStructure
{
    public class SalesContextLoader : ISalesContextLoader
    {
        private const string SalesmanIdentifier = "001";
        private const string CustomerIdentifier = "002";
        private const string SaleIdentifier = "003";

        public SalesContext Load(string filePath)
        {
            var engine = new MultiRecordEngine(typeof(Salesman),
                typeof(Customer),
                typeof(Sale)) {RecordSelector = CustomSelector};

            var records = engine.ReadFile(filePath);

            return TransformToSalesContext(records);
        }

        private SalesContext TransformToSalesContext(object[] records)
        {
            var dictionary = records
                .GroupBy(r => r.GetType().Name)
                .ToDictionary(g => g.Key, g => g.ToList());

            var amountSalesman =
                dictionary.TryGetValue(typeof(Salesman).Name, out var saleObjects)
                    ? saleObjects.Count
                    : 0;
            var amountCustomer = dictionary.TryGetValue(typeof(Customer).Name, out var customerObjects)
                ? customerObjects.Count
                : 0;

            var sales = dictionary.TryGetValue(typeof(Sale).Name, out var salesObjects)
                ? salesObjects.Cast<Sale>().ToList()
                : new List<Sale>();

            return new SalesContext(amountSalesman, amountCustomer, sales);

        }

        private Type CustomSelector(MultiRecordEngine engine, string recordLine)
        {
            if (string.IsNullOrEmpty(recordLine) || recordLine.Length < 3)
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