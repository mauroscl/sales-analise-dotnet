﻿using SalesProcessor.Application.Ports.Driver;
using System.Collections.Generic;
using System.IO;

namespace SalesProcessor.Adapters.Primary
{

    public class FileSaleDataAdapter
    {

        private readonly ISaleDataProcessor _saleDataProcessor;

        public FileSaleDataAdapter(ISaleDataProcessor saleDataProcessor)
        {
            this._saleDataProcessor = saleDataProcessor;
        }

        public void ProcessFile(string filePath)
        {
            var fileContent = File.ReadAllText(filePath);
            _saleDataProcessor.Process(fileContent, Path.GetFileName(filePath));
        }

    }
}
