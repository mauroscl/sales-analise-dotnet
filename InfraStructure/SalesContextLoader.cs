using Business;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra
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
                typeof(SaleItems)) {RecordSelector = CustomSelector};

            object[] records = engine.ReadFile(filePath);

            return TransformToSalesContext(records);
        }

        private SalesContext TransformToSalesContext(object[] records)
        {
            var dictionary = records
                .GroupBy(r => r.GetType().Name)
                .ToDictionary(g => g.Key, g => g.ToList());

            int amountSalesman =
                dictionary.TryGetValue(SalesmanIdentifier, out var saleObjects) 
                    ? saleObjects.Count 
                    : 0;
            int amountCustomer = dictionary.TryGetValue(CustomerIdentifier, out var customerObjects)
                ? customerObjects.Count
                : 0;

            var sales = dictionary.TryGetValue(SaleIdentifier, out var salesObjects)
                ?  TransformToSales(salesObjects)
                : new List<Sale>();

            return new SalesContext(amountSalesman, amountCustomer, sales);

        }

        private IEnumerable<Sale> TransformToSales(IEnumerable<object>  salesObjects)
        {
            return salesObjects.Cast<SaleItems>().Select(TransformToSale);
        }

        private Sale TransformToSale(SaleItems saleItems)
        {
            var sale = new Sale(saleItems.SaleId, saleItems.Salesman);
            var itens = getSaleItems(saleItems.Itens);
            sale.SetItems(itens);
            return sale;
        }

        private IEnumerable<SaleItem> getSaleItems(string serializedItems)
        {
            var engine = new FileHelperEngine<SaleItem>();
            return engine.ReadString(serializedItems.Replace(",", "\r\n"));
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
                    type = typeof(SaleItems);
                    break;
            }

            return type;
        }
    }
}