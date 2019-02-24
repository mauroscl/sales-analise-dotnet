using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessTest
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
        public void MustReturnEmptyListForMostExpensiveSalesWhenNoSailvesAvailable()
        {
            var mostExpensiveSales = _service.CalculateMostExpensiveSales(new List<Sale>());
            Assert.IsFalse(mostExpensiveSales.Any());
        }

    }
}
