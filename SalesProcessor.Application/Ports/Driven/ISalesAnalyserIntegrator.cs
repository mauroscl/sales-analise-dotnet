﻿using System.Collections.Generic;

namespace SalesProcessor.Application.Ports.Driven
{
    public interface ISalesAnalyserIntegrator
    {
        void SendSaleData(string data, string saleKey);

    }
}