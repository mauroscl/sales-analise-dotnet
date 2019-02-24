using System;
using Business;
using FileHelpers;

namespace Infra
{
    public class FileProcessor
    {
        public SaleItem[] Process(string filePath)
        {
            var engine = new FileHelperEngine<SaleItem>();
            return engine.ReadString("003-10-11-11.5");
        }

        public object[] ProcessMultiType()
        {

            var engine = new MultiRecordEngine(typeof(Salesman),
                typeof(Customer),
                typeof(SaleItems));

            engine.RecordSelector = new RecordTypeSelector(CustomSelector);

            const string inputFileContent =
                "001ç1234567891234çDiegoç50000\r\n" +
                "001ç3245678865434çRenatoç40000.99\r\n" +
                "002ç2345675434544345çJose da SilvaçRural\r\n" +
                "002ç2345675433444345çEduardo PereiraçRural\r\n" +
                "003ç10ç1-10-100,2-30-2.50,3-40-3.10]çDiego\r\n" +
                "003ç08ç[1-34-10,2-33-1.50,3-40-0.10]çRenato";

            return engine.ReadString(inputFileContent);
        }

        private Type CustomSelector(MultiRecordEngine engine, string recordLine)
        {
            if (string.IsNullOrEmpty(recordLine) || recordLine.Length < 3)
                return null;

            var typeIdentifier = recordLine.Substring(0, 3);
            Type type = null;
            switch (typeIdentifier)
            {
                case "001":
                    type = typeof(Salesman);
                    break;
                case "002":
                    type = typeof(Customer);
                    break;
                case "003":
                    type = typeof(SaleItems);
                    break;
            }

            return type;
        }
    }
}
