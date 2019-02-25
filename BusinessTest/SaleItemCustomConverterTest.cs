using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessTest
{
    [TestClass]
    public class SaleItemCustomConverterTest
    {
        private readonly SaleItemCustomConverter _saleItemCustomConverter = new SaleItemCustomConverter();

        [TestMethod]
        public void MustReturnErrorWhenItemsNoWrappedByBrackets()
        {
            try
            {
                _saleItemCustomConverter.StringToField("10-20-30");
                Assert.Fail("Must throw Exception because items had no brackets");

            }
            catch (Exception e)
            {
                Assert.AreEqual("Sale Item must start and terminate with brackets: 10-20-30", e.Message);
            }
        }

        [TestMethod]
        public void MustReturnErrorWhenItemHasNotThreeValues()
        {
            try
            {
                _saleItemCustomConverter.StringToField("[20-30]");

                Assert.Fail("Must throw Exception because item has less than three values");

            }
            catch (Exception e)
            {
                Assert.AreEqual("Sale item must have 3 values: 20-30", e.Message);
            }

        }

        [TestMethod]
        public void MustConvertStringToSaleItem()
        {
            var saleItems = ((IList) _saleItemCustomConverter.StringToField("[1-20-30,2-30-50.1]")).Cast<SaleItem>().ToList();
            Assert.AreEqual(2, saleItems.Count());

            var saleItem1 = saleItems.ElementAt(0);
            Assert.AreEqual("1", saleItem1.Id);
            Assert.AreEqual(20, saleItem1.Quantity);
            Assert.AreEqual(30, saleItem1.Price);

            var saleItem2 = saleItems.ElementAt(1);
            Assert.AreEqual("2", saleItem2.Id);
            Assert.AreEqual(30, saleItem2.Quantity);
            Assert.AreEqual(50.1, saleItem2.Price);

        }

    }
}
