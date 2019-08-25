using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesProcessor.Adapters.Primary;

namespace SalesProcesssor.Adapters.Test.Primary
{
    [TestClass]
    public class FileServiceTest
    {
        [TestMethod]
        public void MustCalculateStatisticsFileName()
        {
            var fileService = new FileService();
            var statisticsFileName = fileService.GetStatisticsFileName("data\\in\\sales.dat", "data\\out");
            Assert.AreEqual("data\\out\\sales.done.dat", statisticsFileName);
        }
    }
}