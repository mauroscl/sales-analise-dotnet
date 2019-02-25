using Business;
using FileHelpers;
using System.Collections.Generic;

namespace InfraStructure
{
    public class SalesSummaryOutputFileService : ISalesSummaryOutputService
    {
        public void Write(string path, SalesSummary salesSummary)
        {
            var engine = new FileHelperEngine<SalesSummary>();
            engine.WriteFile(path, new List<SalesSummary>{salesSummary});
        }
    }
}
