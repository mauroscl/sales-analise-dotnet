using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SalesAnalyzer.Application.Domain;
using SalesAnalyzer.Application.Ports.Driven;
using SalesAnalyzer.Application.Test.DataProvider;
using SalesAnalyzer.Application.UseCases;

namespace SalesAnalyzer.Application.Test.UseCases
{
    [TestClass]
    public class SaleCsvProcessorTest
    {
        [TestMethod]
        public void MustProcessCsvAndWriteStatistics()
        {
            var salesContextLoaderMock = new Mock<ISalesContextLoader>();

            var sales = SaleDataProvider.GenerateMostExpensiveSalesTied();

            var salesContext = new SalesContext(3, 1, sales);

            salesContextLoaderMock.Setup(x => x.LoadCsv(It.IsAny<string>()))
                .Returns(salesContext);

            var saleCsvProcessor = new SaleCsvProcessor(salesContextLoaderMock.Object, new SalesStatisticsService());

            saleCsvProcessor.Process("data/in/teste.dat");

            salesContextLoaderMock.Verify(x => x.LoadCsv(It.IsAny<string>()), Times.Once);

        }
    }
}