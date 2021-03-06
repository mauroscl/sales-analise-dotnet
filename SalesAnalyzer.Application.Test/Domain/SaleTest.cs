﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesAnalyzer.Application.Domain;

namespace SalesAnalyzer.Application.Test.Domain
{
    [TestClass]
    public class SaleTest
    {
        [TestMethod]
        public void MustCalculateTotal()
        {
            var saleItem1 = new SaleItem("001", 10, 3);
            var saleItem2 = new SaleItem("002", 5, 3.25);

            var sale = new Sale("S001", "Mauro");
            sale.SetItems(new List<SaleItem> {saleItem1, saleItem2});

            Assert.AreEqual(46.25, sale.Total);
        }
    }
}