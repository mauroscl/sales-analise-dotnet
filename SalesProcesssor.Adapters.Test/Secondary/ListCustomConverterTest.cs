using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesProcessor.Adapters.Secondary;

namespace SalesProcesssor.Adapters.Test.Secondary
{
    [TestClass]
    public class ListCustomConverterTest
    {
        [TestMethod]
        public void MustSerializeListOfStrings()
        {
            var listCustomConverter = new ListCustomConverter();
            var valueToSerialize = new List<string> {"001", "002", "003"};
            var fieldToString = listCustomConverter.FieldToString(valueToSerialize);
            Assert.AreEqual("[001,002,003]", fieldToString);
        }
    }
}