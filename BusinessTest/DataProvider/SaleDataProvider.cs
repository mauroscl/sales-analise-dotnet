﻿using System;
using System.Collections.Generic;
using System.Text;
using Business;
// ReSharper disable All

namespace BusinessTest.DataProvider
{
    class SaleDataProvider
    {
        public static IList<Sale> getTwoSales()
        {
            Sale sale1 = new Sale("01", "Mauro");

            SaleItem saleItem1 = new SaleItem("001", 10, 50);

            SaleItem saleItem2 = new SaleItem("002", 3, 180);

            //total = 10*50 + 3*180 = 1040
            sale1.SetItems(new List<SaleItem>(){saleItem1, saleItem2});

            Sale sale2 = new Sale("02", "João");

            SaleItem saleItem3 = new SaleItem("001", 12, 45);

            SaleItem saleItem4 = new SaleItem("002", 10, 34);

            //total = 12*45 + 10*34 = 880
            sale2.setItems(List.of(saleItem3, saleItem4));

            return List.of(sale1, sale2);
        }

    }
}
