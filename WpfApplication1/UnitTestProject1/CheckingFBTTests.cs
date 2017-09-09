using AppelWRE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;
using System.Linq;
using PricingLibrary.Utilities.MarketDataFeed;

namespace FBT.Tests
{
    [TestClass()]
    public class CheckingFBTTest
    {
        [TestMethod()]
        public void VanillaTest()
        {
            var debut = new DateTime(2017, 9, 6);
            var duree = 365;
            var pas = 1;
            var window = 50;

            var init = new HardCodeInitializer();
            var marketSimulator = new SimulatedDataFeedProvider();
            //var marketSimulator = new HistDataFeedProvider();

            var opt = init.initVanillaOpt(debut, duree - 1);
            var vanillaOpt = new VanillaComputation(opt);

            var res = vanillaOpt.GenChartData(window, debut, pas, marketSimulator);

            Console.WriteLine("Demarrer \n");
            Console.WriteLine("Financial product: " + vanillaOpt.Option.Name + "\n");
            for (int i = 0; i < res.OptionPrice.Count; i++)
            {
                Console.WriteLine("Date: " + vanillaOpt.MarketDataDates[window + i]);
                Console.WriteLine("Option price: " + res.OptionPrice[i]);
                Console.WriteLine("Portfolio value:" + res.PortfolioValue[i].Value);
                var trackErr = res.OptionPrice[i] - res.PortfolioValue[i].Value;
                Console.WriteLine("Tracking error: " + trackErr);
                Console.WriteLine("Composition of the portfolio: ");
                Console.WriteLine("   Number of share " + opt.UnderlyingShare.Name + ": " + res.PortfolioValue[i].Deltas[0]);
                Console.WriteLine("   Invested in at a free risk rate:" + res.PortfolioValue[i].FreeRiskDelta + "\n");
            }
            Console.WriteLine("At Maturity:");
            Console.WriteLine("Maturity Date: " + vanillaOpt.Option.Maturity);
            var dic = new Dictionary<string, decimal>();
            dic.Add(vanillaOpt.Option.UnderlyingShareIds[0], (decimal)vanillaOpt.Spots.Last()[0]);
            Console.WriteLine("Payoff: " + opt.GetPayoff(dic) + "\n");
            Console.WriteLine("End");

        }

        [TestMethod()]
        public void BasketTest()
        {
            var debut = new DateTime(2017, 9, 6);
            var duree = 365;
            var pas = 1;
            var window = 50;

            var init = new HardCodeInitializer();
            var marketSimulator = new SimulatedDataFeedProvider();
            //var marketSimulator = new HistDataFeedProvider();

            var opt = init.initBasketOpt(debut, duree - 1);
            var basketOpt = new BasketComputation(opt);

            var res = basketOpt.GenChartData(window, debut, pas, marketSimulator);


            Console.WriteLine("Demarrer \n");
            Console.WriteLine("Financial product: " + basketOpt.Option.Name + "\n");
            for (int date = 0; date < res.OptionPrice.Count; date++)
            {
                Console.WriteLine("Date: " + basketOpt.MarketDataDates[window + date]);
                Console.WriteLine("Option price: " + res.OptionPrice[date]);
                Console.WriteLine("Portfolio value:" + res.PortfolioValue[date].Value);
                var trackErr = res.OptionPrice[date] - res.PortfolioValue[date].Value;
                Console.WriteLine("Tracking error: " + trackErr);
                for (var shareId = 0; shareId < opt.UnderlyingShareIds.Length; shareId ++)
                {
                    Console.WriteLine("   Nb of share of ID '" + opt.UnderlyingShareIds[shareId] + "': " + res.PortfolioValue[date].Deltas[shareId]);
                }
                Console.WriteLine("   Value invested at a free risk rate:" + res.PortfolioValue[date].FreeRiskDelta + "\n");
            }

            Console.WriteLine("At Maturity:");
            Console.WriteLine("Maturity Date: " + basketOpt.Option.Maturity);
            var dic = new Dictionary<string, decimal>();
            for (var shareId = 0; shareId < opt.UnderlyingShareIds.Length; shareId++)
            {
                dic.Add(opt.UnderlyingShareIds[shareId], (decimal)basketOpt.Spots.Last()[shareId]);
            }
            Console.WriteLine("Payoff: " + opt.GetPayoff(dic) + "\n");
            Console.WriteLine("End");

        }


    }
}