using System;
using System.Collections.Generic;
using System.Text;
using Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessTest
{
    [TestClass]
    public class SaleItemTest
    {
        [TestMethod]
        public void MustCalculateTotal()
        {
            var saleItem = new SaleItem("001", 10, 5.25);
            Assert.AreEqual(52.5, saleItem.Total);
        }
    }
}
