using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SalesAnalyzer.Application.Domain;

namespace SalesAnalyzer.Adapters.Test
{
    [TestClass]
    public class SalesContextLoaderTest
    {
        [TestMethod]
        public void MustLoadContextFromEmptyFile()
        {
            var fileHelperEngineMock = new Mock<IFileHelperEngine>();
            fileHelperEngineMock.Setup(x => x.ReadCsvFile(It.IsAny<string>())).Returns(new object[] { });
            var salesContextLoader = new SalesContextLoader(fileHelperEngineMock.Object);

            var salesContext = salesContextLoader.Load("data/in/sale.dat");

            Assert.AreEqual(0, salesContext.AmountSalesman);
            Assert.AreEqual(0, salesContext.AmountCustomer);
            Assert.IsFalse(salesContext.Sales.Any());

            fileHelperEngineMock.Verify(x => x.ReadCsvFile(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void MustLoadContextFromFullFiles()
        {
            var salesman = new Salesman {Name = "Mauro", Cpf = "xxx", Salary = 1000};
            var customer = new Customer
            {
                Cnpj = "8684",
                Name = "Enterprise",
                BusinessArea = "Rural"
            };
            var sale = new Sale("001", "Mauro");
            var saleItem = new SaleItem("001", 10, 20);
            sale.SetItems(new List<SaleItem> {saleItem});

            var objects = new object[] {salesman, customer, sale};

            var fileHelperEngineMock = new Mock<IFileHelperEngine>();
            fileHelperEngineMock.Setup(x => x.ReadCsvFile(It.IsAny<string>())).Returns(objects);
            var salesContextLoader = new SalesContextLoader(fileHelperEngineMock.Object);

            var salesContext = salesContextLoader.Load("data/in/sale.dat");

            Assert.AreEqual(1, salesContext.AmountSalesman);
            Assert.AreEqual(1, salesContext.AmountCustomer);
            Assert.AreEqual(1, salesContext.Sales.Count);

            fileHelperEngineMock.Verify(x => x.ReadCsvFile(It.IsAny<string>()), Times.Once);
        }
    }
}