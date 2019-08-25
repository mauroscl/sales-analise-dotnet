using System.Collections.Generic;
using System.Linq;
using Business.Domain;
using Business.Ports;

namespace Adapters
{
    public class SalesContextLoader : ISalesContextLoader
    {
        private readonly IFileHelperEngine _fileHelperEngine;

        public SalesContextLoader(IFileHelperEngine fileHelperEngine)
        {
            _fileHelperEngine = fileHelperEngine;
        }

        public SalesContext Load(string filePath)
        {
            var records = _fileHelperEngine.ReadCsvFile(filePath);
            return TransformToSalesContext(records);
        }

        public SalesContext LoadCsv(string csvContent)
        {
            var records = _fileHelperEngine.ReadCsv(csvContent);
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
    }
}