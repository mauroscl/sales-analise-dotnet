using System;
using System.Collections.Generic;
using System.Text;
using Business;
using Business.Domain;

// ReSharper disable All

namespace BusinessTest.DataProvider
{
    internal class SaleDataProvider
    {
        internal static IList<Sale> GenerateTwoSales()
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
            sale2.SetItems(new List<SaleItem>() { saleItem3, saleItem4 });

            return new List<Sale>{sale1, sale2};
        }

        internal static List<Sale> GenerateWorstSellersTied()
        {
            Sale sale1 = new Sale("01", "Mauro");

            SaleItem saleItem1 = new SaleItem("001", 10, 50);
            //total = 500
            sale1.SetItems(new List<SaleItem>{saleItem1});

            Sale sale2 = new Sale("02", "João");

            SaleItem saleItem2 = new SaleItem("002", 2, 250);
            //total = 500
            sale2.SetItems(new List<SaleItem>{ saleItem2 });

            Sale sale3 = new Sale("03", "Paulo");

            SaleItem saleItem3 = new SaleItem("003", 2, 300);
            //total = 600
            sale3.SetItems(new List<SaleItem>{ saleItem3 });

            return new List<Sale>{sale1, sale2, sale3};
        }

        internal static List<Sale> GenerateMostExpensiveSalesTied()
        {
            Sale sale1 = new Sale("01", "Mauro");

            SaleItem saleItem1 = new SaleItem("001", 10, 50);
            //total = 500
            sale1.SetItems(new List<SaleItem> {saleItem1});

            Sale sale2 = new Sale("02", "João");

            SaleItem saleItem2 = new SaleItem("002", 3, 300);
            //total = 900
            sale2.SetItems(new List<SaleItem>{ saleItem2 });

            Sale sale3 = new Sale("03", "Paulo");

            SaleItem saleItem3 = new SaleItem("003", 2, 450);
            //total = 900
            sale3.SetItems(new List<SaleItem>{saleItem3});

            return new List<Sale>{ sale1, sale2, sale3 };
        }

    }
}
