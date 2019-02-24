using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface ISalesContextLoader
    {
        SalesContext Load(string filePath);
    }
}
