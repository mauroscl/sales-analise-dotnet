﻿using FileHelpers;
using System.Collections.Generic;

namespace Business
{
    [DelimitedRecord(";")]
    public class SalesSummary
    {
        public SalesSummary(int amountSalesman, int amountCustomer, IList<string> worstSellers, IList<string> mostExpensiveSales)
        {
            AmountSalesman = amountSalesman;
            AmountCustomer = amountCustomer;
            WorstSellers = worstSellers;
            MostExpensiveSales = mostExpensiveSales;
        }

        protected SalesSummary() {}

        private int AmountSalesman { get; }
        private int AmountCustomer { get; }

        [FieldConverter(typeof(ListCustomConverter))]
        public IList<string> WorstSellers { get;  }
        [FieldConverter(typeof(ListCustomConverter))]
        public IList<string> MostExpensiveSales { get;  }
    }
}
