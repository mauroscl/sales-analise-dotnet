using System.Collections.Generic;
using System.Net;
using FileHelpers;
using SalesProcessor.Application.UseCases;

namespace SalesProcessor.Adapters.Secondary
{
    [DelimitedRecord(";")]
    public class SalesSummaryForSerialization
    {
        private SalesSummaryForSerialization(int amountSalesman, int amountCustomer, IList<string> worstSellers,
            IList<string> mostExpensiveSales)
        {
            AmountSalesman = amountSalesman;
            AmountCustomer = amountCustomer;
            WorstSellers = worstSellers;
            MostExpensiveSales = mostExpensiveSales;
        }

        protected SalesSummaryForSerialization()
        {
        }

        public static SalesSummaryForSerialization FromSaleSummary(SalesSummary salesSummary)
        {
            return new SalesSummaryForSerialization(salesSummary.AmountSalesman, salesSummary.AmountCustomer, 
                salesSummary.WorstSellers, salesSummary.MostExpensiveSales);
        }

        public int AmountSalesman { get; }
        public int AmountCustomer { get; }

        [FieldConverter(typeof(ListCustomConverter))]
        public IList<string> WorstSellers { get; }

        [FieldConverter(typeof(ListCustomConverter))]
        public IList<string> MostExpensiveSales { get; }
    }
}