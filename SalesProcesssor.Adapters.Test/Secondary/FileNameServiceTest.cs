using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesProcessor.Adapters.Secondary;

namespace SalesProcesssor.Adapters.Test.Secondary
{
    [TestClass]
    public class FileNameServiceTest
    {
        [TestMethod]
        public void MustCalculateStatisticsFileName()
        {
            var fileNameService = new FileNameService();
            var statisticsFileName = fileNameService.GetStatisticsFileName("data\\in\\sales.dat", "data\\out");
            Assert.AreEqual("data\\out\\sales.done.dat", statisticsFileName);
        }
    }
}