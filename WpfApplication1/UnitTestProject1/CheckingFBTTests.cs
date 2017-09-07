using AppelWRE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;

namespace FBT.Tests
{
    [TestClass()]
    public class CheckingFBTTest
    {
        [TestMethod()]
        public void VanillaTest()
        {
            var init = new HardCodeInitializer();

            var debut = new DateTime(2017, 9, 6);
            uint duree = 365;
            uint pas = 2;

            var opt = init.initVanillaOpt(debut, duree - 1);
            var vanillaOpt = new VanillaComputation(opt, debut);

            var riskFreeRate = init.initRiskFreeRate(pas);
            var dates = init.getRebalancingDates(debut, duree - 1, pas);

            var res = vanillaOpt.computePrice(dates);
            var port = vanillaOpt.computeValuePortfolio(dates, dates, riskFreeRate);
            var j = res.Count;



            Console.WriteLine("Demarrer \n");
            Console.WriteLine("Financial product: " + opt.Name + "\n");
            for (int i = 0; i < j; i++)
            {
                Console.WriteLine("Date: " + res[i].Date);
                Console.WriteLine("Option price: " + res[i].Price);
                Console.WriteLine("Portfolio value:" + port[i].Price);
                var trackErr = res[i].Price - port[i].Price;
                Console.WriteLine("Tracking error: " + trackErr);
                Console.WriteLine("Composition of the portfolio: ");
                Console.WriteLine("   Value in the underlying share " + opt.UnderlyingShare.Name + ": " + port[i].valShares[0]);
                Console.WriteLine("   Value invested at a free risk rate:" + port[i].valSansRisque + "\n");
            }
            Console.WriteLine("At Maturity:");
            Console.WriteLine("Maturity Date: " + opt.Maturity);
            var dic = new Dictionary<string, decimal>();
            dic.Add(opt.UnderlyingShare.Id, (decimal)vanillaOpt.spots[opt.Maturity]);
            Console.WriteLine("Payoff: " + opt.GetPayoff(dic) + "\n");
            Console.WriteLine("End");

        }

        [TestMethod()]
        public void BasketTest()
        {
            var init = new HardCodeInitializer();

            var debut = new DateTime(2017, 9, 6);
            var duree = 365;
            var pas = 2;

            var opt = init.initBasketOpt(debut, duree - 1);
            var basketOpt = new BasketComputation(opt, debut);

            var riskFreeRate = init.initRiskFreeRate(pas);
            var dates = init.getRebalancingDates(debut, duree - 1, pas);

            var res = basketOpt.computePrice(dates);
            var port = basketOpt.computeValuePortfolio(dates, dates, riskFreeRate);
            var j = res.Count;



            Console.WriteLine("Demarrer \n");
            Console.WriteLine("Financial product: " + opt.Name + "\n");
            for (int i = 0; i < j; i++)
            {
                Console.WriteLine("Date: " + res[i].Date);
                Console.WriteLine("Option price: " + res[i].Price);
                Console.WriteLine("Portfolio value:" + port[i].Price);
                var trackErr = res[i].Price - port[i].Price;
                Console.WriteLine("Tracking error: " + trackErr);
                Console.WriteLine("Composition of the portfolio: ");
                var shareNb = 0;
                foreach (string id in opt.UnderlyingShareIds)
                {
                    Console.WriteLine("   Value in the underlying share of ID " + id + ": " + port[i].valShares[shareNb]);
                    shareNb++;
                }
                Console.WriteLine("   Value invested at a free risk rate:" + port[i].valSansRisque + "\n");
            }

            Console.WriteLine("At Maturity:");
            Console.WriteLine("Maturity Date: " + opt.Maturity);
            var dic = new Dictionary<string, decimal>();
            var k = 0;
            foreach (string id in opt.UnderlyingShareIds)
            {
                dic.Add(id, (decimal)basketOpt.spots[opt.Maturity][k]);
                k++;
            }
            Console.WriteLine("Payoff: " + opt.GetPayoff(dic) + "\n");
            Console.WriteLine("End");

        }


    }
}