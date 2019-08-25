using System.Collections.Generic;
using FileHelpers;
using InfraStructure;
using SalesProcessor.Application.UseCases;

namespace SalesProcessor.Adapters.Secondary
{
    public class SalesSummaryOutputFileService : ISalesSummaryOutputService
    {
        public void Write(string path, SalesSummary salesSummary)
        {
            var salesSummaryForSerialization = SalesSummaryForSerialization.FromSaleSummary(salesSummary);
            var engine = new FileHelperEngine<SalesSummaryForSerialization>();
            engine.WriteFile(path, new List<SalesSummaryForSerialization> {salesSummaryForSerialization});
        }
    }
}