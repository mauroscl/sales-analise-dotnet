using System.IO;

namespace InfraStructure
{
    public class SaleFileReader
    {
        private string ReadContent(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}