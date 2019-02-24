using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface ISaleDataProcessor
    {
        void Process(string filePath);
    }
}
