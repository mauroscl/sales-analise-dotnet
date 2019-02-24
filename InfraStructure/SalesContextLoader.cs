using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Business;
using FileHelpers;

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
                typeof(SaleItems)) {RecordSelector = CustomSelector};

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

            var sales = dictionary.TryGetValue(typeof(SaleItems).Name, out var salesObjects)
                ?  TransformToSales(salesObjects)
                : new List<Sale>();

            return new SalesContext(amountSalesman, amountCustomer, sales);

        }

        private IEnumerable<Sale> TransformToSales(IList<object>  salesObjects)
        {
            var sales = new List<Sale>();
            foreach (var saleItems in salesObjects.Cast<SaleItems>())
            {
                try
                {
                    sales.Add(TransformToSale(saleItems)); 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return sales;
        }

        private Sale TransformToSale(SaleItems saleItems)
        {
            var sale = new Sale(saleItems.SaleId, saleItems.Salesman);
            var itens = TransformToSaleItems(saleItems.SerializedItems);
            sale.SetItems(itens);
            return sale;
        }

        private IEnumerable<SaleItem> TransformToSaleItems(string serializedItems)
        {
            var regex = new Regex("^\\[.+\\]$");
            if (!regex.IsMatch(serializedItems))
            {
                throw new Exception("Invalid Sale Items format: " + serializedItems);
            }

            var engine = new FileHelperEngine<SaleItem>();
            return engine.ReadString(serializedItems.Substring(1, serializedItems.Length -2).Replace(",", "\r\n"));
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