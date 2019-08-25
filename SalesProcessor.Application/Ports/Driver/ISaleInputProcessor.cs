using System.Collections.Generic;

namespace SalesProcessor.Application.Ports.Driver
{
    public interface ISaleInputProcessor
    {
        void Process(string saleContent, IReadOnlyDictionary<string, string> headers);
    }
}