using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var fileProcessor = new FileProcessor();
            var saleItems = fileProcessor.Process("");
            Assert.AreEqual(1, saleItems.Length);
            //10-11-11.5"
            var saleItem = saleItems[0];
            Assert.AreEqual("10", saleItem.Id);
            Assert.AreEqual(11, saleItem.Quantity);
            Assert.AreEqual(11.5, saleItem.Price);
        }

        [TestMethod]
        public void TestMultiType()
        {
            var fileProcessor = new FileProcessor();
            var records = fileProcessor.ProcessMultiType();
            Assert.AreEqual(6, records.Length);

            var dictionary = records
                .GroupBy(r => r.GetType().Name)
                .ToDictionary(g => g.Key, g => g.ToList());

            var salesItemObjects = dictionary["SaleItems"];

            var saleItems = salesItemObjects.Cast<SaleItems>();

            var regex = new Regex("^\\[.+\\]$");

            foreach (var saleItem in saleItems)
            {
                Console.WriteLine(saleItem.Itens + " - Match: " + regex.IsMatch(saleItem.Itens)); 
            }

            //var x = new Dictionary<string, List<object> >();
            //x.TryGetValue("001", out var y);

            //var groupedList =
            //    (from record in records
            //        group record by record.GetType().Name
            //        into grouping
            //        select new 
            //        {
            //            Type = grouping.Key,
            //            Records = grouping.ToList()
            //        }).ToList();

            //foreach (var group in groupedList)
            //{
            //    Console.WriteLine("Type=" + group.Type + " - Size: " + group.Records.Count);
            //}

        }
    }
}