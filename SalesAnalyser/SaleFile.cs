using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Text;

namespace SalesAnalyser
{
    class SaleFile
    {
        internal SaleFile(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
        }

        public string FileName { get; }
        public string Content { get; }

    }
}
