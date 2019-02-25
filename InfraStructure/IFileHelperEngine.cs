using System.Collections.Generic;
using System.Text;

namespace InfraStructure
{
    public interface IFileHelperEngine
    {
        object[] ReadCsvFile(string filePath);
    }
}
