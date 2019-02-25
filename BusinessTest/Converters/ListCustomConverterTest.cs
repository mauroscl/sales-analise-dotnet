using System;
using System.Collections.Generic;
using Business.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessTest.Converters
{
    [TestClass]
    public class ListCustomConverterTest
    {
        [TestMethod]
        public void MustSerializeListOfStrings()
        {
            var listCustomConverter = new ListCustomConverter();
            var valueToSerialize = new List<String> {"001", "002", "003"};
            var fieldToString = listCustomConverter.FieldToString(valueToSerialize);
            Assert.AreEqual("[001,002,003]", fieldToString);
        }
    }
}
