using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesAnalyzer.Application.Domain;
using SalesAnalyzer.Application.Test.DataProvider;

namespace SalesAnalyzer.Application.Test.Domain
{
    [TestClass]
    public class SalesStatisticsServiceTest
    {
        private readonly SalesStatisticsService _service = new SalesStatisticsService();

        [TestMethod]
        public void MustReturnEmptyListForWorstSellersWhenNoSalesAvailable()
        {
            var worstSellers = _service.CalculateWorstSellers(new List<Sale>());
            Assert.IsFalse(worstSellers.Any());
        }

        [TestMethod]
        public void MustReturnNameOfWorstSeller()
        {
            var sales = SaleDataProvider.GenerateTwoSales();
            var worstSellers = _service.CalculateWorstSellers(sales);
            Assert.AreEqual(1, worstSellers.Count);
            Assert.AreEqual("João", worstSellers.First());
        }

        [TestMethod]
        public void MustReturnNamesOfWorstSellersWhenTied()
        {
            var sales = SaleDataProvider.GenerateWorstSellersTied();
            var worstSellers = _service.CalculateWorstSellers(sales);
            Assert.AreEqual(2, worstSellers.Count);
            Assert.IsTrue(worstSellers.Contains("João"));
            Assert.IsTrue(worstSellers.Contains("Mauro"));
        }

        [TestMethod]
        public void MustReturnEmptyListForMostExpensiveSalesWhenNoSailvesAvailable()
        {
            var mostExpensiveSales = _service.CalculateMostExpensiveSales(new List<Sale>());
            Assert.IsFalse(mostExpensiveSales.Any());
        }

        [TestMethod]
        public void MustdReturnIdOfMostExpensiveSale()
        {
            var sales = SaleDataProvider.GenerateTwoSales();
            var mostExpensiveSales = _service.CalculateMostExpensiveSales(sales);
            Assert.AreEqual(1, mostExpensiveSales.Count);
            Assert.AreEqual("01", mostExpensiveSales.First());
        }

        [TestMethod]
        public void MustReturnIdOfMostExpensiveSalesWhenTied()
        {
            var sales = SaleDataProvider.GenerateMostExpensiveSalesTied();
            var mostExpensiveSales = _service.CalculateMostExpensiveSales(sales);
            Assert.AreEqual(2, mostExpensiveSales.Count);
            Assert.IsTrue(mostExpensiveSales.Contains("02"));
            Assert.IsTrue(mostExpensiveSales.Contains("03"));
        }
    }
}