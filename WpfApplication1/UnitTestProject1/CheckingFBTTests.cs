using AppelWRE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WpfApplication1.Model.FinancialModel;
using WpfApplication1.Model.Initializer;
using WpfApplication1.Tests;

namespace WpfApplication1.Tests
{
    [TestClass()]
    public class CheckingFBTTest
    {

        
        [TestMethod()]
        public void MainTest()
        {
            var init = new HardCodeInitializer();
            var calculator = new FinancialComputation();


            var debut = new DateTime(2017, 9, 6);
            var duree = 365;
            var pas = 2;

            var opt = init.initOptionsUnivers(debut, duree - 1);
            var dataFeedProvider = new MyDataFeed();
            var spot = dataFeedProvider.GenerateDataFeed(debut, duree - 1, opt);


            var list = calculator.computeListVolatility(10, spot, duree);
            Console.WriteLine(list.Count);
            foreach ( double d in list)
            {
                Console.WriteLine(d);
            }


            /*double[,] tab = new double[2,1];

            for (var k=1; k<3; k++)
            {
                tab[k-1,0] = Math.Log(spot[k]/spot[k-1]);
            }


            var B = Math.Sqrt(PricingLibrary.Utilities.DayCount.ConvertToDouble(1,365));


            double[,] myVol = WRE.computeVolatility(tab);

            Console.WriteLine(myVol[0,0]/B);*/

            var riskFreeRate = init.initRiskFreeRate(pas);

            var dates = init.getDatesOfSimuData(debut, duree - 1, pas);
            var vol = init.getVolatilityOfSimuData(duree - 1, pas);

            var res = calculator.computeDeltasAndPrice(dates, opt, spot, vol, pas);
            var port = calculator.computePricePortfolio(dates, res, spot, riskFreeRate, pas);
            var j = res.Count;



            Console.WriteLine("Demarrer \n");
            Console.WriteLine("Financial product: " + opt.Name + "\n");
            for (int i =0; i < j; i++)
            {
                Console.WriteLine("Date: " + res[i].Date);
                Console.WriteLine("Option price: " + res[i].Price);
                Console.WriteLine("Portfolio value:" + port[i].Price);
                var trackErr = res[i].Price - port[i].Price;
                Console.WriteLine("Tracking error: " + trackErr);
                Console.WriteLine("Composition of the portfolio: ");
                Console.WriteLine("   Value in the underlying share " + opt.UnderlyingShare.Name + ": " + port[i].valShare);
                Console.WriteLine("   Value invested at a free risk rate:" + port[i].valSansRisque + "\n");
            }
            Console.WriteLine("At Maturity:");
            Console.WriteLine("Maturity Date: " + opt.Maturity);
            var dic = new Dictionary<string, decimal>();
            dic.Add(opt.UnderlyingShare.Id, (decimal)spot[j]);
            Console.WriteLine("Payoff: " + opt.GetPayoff(dic) + "\n");
            Console.WriteLine("End");

        }

        


    }
}