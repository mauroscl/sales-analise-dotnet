using Business.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessTest.Domain
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