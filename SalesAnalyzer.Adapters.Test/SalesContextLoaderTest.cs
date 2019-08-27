using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesAnalyzer.Adapters.Secondary;
using System;
using System.Linq;

namespace SalesAnalyzer.Adapters.Test
{
    [TestClass]
    public class SalesContextLoaderTest
    {
        [TestMethod]
        public void MustLoadContextFromEmptyContent()
        {

            var salesContextLoader = new SalesContextLoader(new SalesFileHelperEngine());

            var salesContext = salesContextLoader.LoadCsv("");

            Assert.AreEqual(0, salesContext.AmountSalesman);
            Assert.AreEqual(0, salesContext.AmountCustomer);
            Assert.IsFalse(salesContext.Sales.Any());

        }

        [TestMethod]
        public void MustLoadContextFromFullContent()
        {

            var salesContextLoader = new SalesContextLoader(new SalesFileHelperEngine());

            var content = "001ç9684448448744çMauroç50000" + Environment.NewLine +
                          "002ç2345675434544345çJose da SilvaçRural" + Environment.NewLine +
                          "003ç10ç[1-10-100,2-30-2.50,3-40-3.10]çMauro";

            var salesContext = salesContextLoader.LoadCsv(content);

            Assert.AreEqual(1, salesContext.AmountSalesman);
            Assert.AreEqual(1, salesContext.AmountCustomer);
            Assert.AreEqual(1, salesContext.Sales.Count);

            var sale = salesContext.Sales[0];
            Assert.AreEqual("Mauro", sale.Salesman);
            Assert.AreEqual("10", sale.SaleId);

            Assert.AreEqual(3, sale.Items.Count());

            var firstItem = sale.Items.First();

            Assert.AreEqual("1", firstItem.Id);
            Assert.AreEqual(10,firstItem.Quantity);
            Assert.AreEqual(100, firstItem.Price);

        }
    }
}