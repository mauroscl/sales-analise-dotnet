using SalesProcessor.Application.Ports.Driver;
using System.Collections.Generic;
using System.IO;

namespace SalesProcessor.Adapters.Primary
{

    public class FileSaleInputAdapter
    {

        private readonly ISaleInputProcessor _saleInputProcessor;

        public FileSaleInputAdapter(ISaleInputProcessor saleInputProcessor)
        {
            this._saleInputProcessor = saleInputProcessor;
        }

        public void ProcessFile(string filePath)
        {
            var headers = new Dictionary<string, string> { { KafkaConfig.FileNameHeader, Path.GetFileName(filePath) } };
            var fileContent = File.ReadAllText(filePath);
            _saleInputProcessor.Process(fileContent, headers);
        }

    }
}
