using SalesProcessor.Application.Ports.Driver;
using System.Collections.Generic;
using System.IO;

namespace SalesProcessor.Adapters.Primary
{

    public class FileSaleProcessor
    {

        private static readonly string FileNameHeader = "CTM_FILE_NAME";

        private readonly ISaleInputProcessor _saleInputProcessor;

        public FileSaleProcessor(ISaleInputProcessor saleInputProcessor)
        {
            this._saleInputProcessor = saleInputProcessor;
        }

        public void ProcessFile(string filePath)
        {
            var headers = new Dictionary<string, string> { { FileNameHeader, Path.GetFileName(filePath) } };
            var fileContent = File.ReadAllText(filePath);
            _saleInputProcessor.Process(fileContent, headers);
        }

    }
}
