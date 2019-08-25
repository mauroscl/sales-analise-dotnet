using Business.Domain;
using Business.Ports;
using Business.UseCases;
using BusinessTest.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessTest.UseCases
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

            //var outputMock = new Mock<ISalesSummaryOutputService>();
            //outputMock
            //    .Setup(x => x.Write(It.IsAny<string>(), It.IsAny<SalesSummary>()))
            //    .Callback<string, SalesSummary>((path, summary) =>
            //    {
            //        Assert.AreEqual(3, summary.AmountSalesman);
            //        Assert.AreEqual(1, summary.AmountCustomer);
            //        Assert.IsTrue(summary.MostExpensiveSales.Contains("02"));
            //        Assert.IsTrue(summary.MostExpensiveSales.Contains("03"));
            //        Assert.IsTrue(summary.WorstSellers.Contains("Mauro"));
            //    });

            //var fileServiceMock = new Mock<IFileService>();
            //fileServiceMock
            //    .Setup(x => x.GetStatisticsFileName(It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns("path");
            //fileServiceMock.Setup(x => x.MoveProcessedFile(It.IsAny<string>()));

            var saleCsvProcessor = new SaleCsvProcessor(salesContextLoaderMock.Object, new SalesStatisticsService());

            saleCsvProcessor.Process("data/in/teste.dat", "data/out");

            salesContextLoaderMock.Verify(x => x.LoadCsv(It.IsAny<string>()), Times.Once);

            //outputMock.Verify(x => x.Write(It.IsAny<string>(), It.IsAny<SalesSummary>()),Times.Once);

            //fileServiceMock
            //    .Verify(x => x.GetStatisticsFileName(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            //fileServiceMock.Verify(x => x.MoveProcessedFile(It.IsAny<string>()), Times.Once);
        }
    }
}