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
            var marketSimulator = new SimulatedDataFeedProvider();
            //var marketSimulator = new HistDataFeedProvider();

            var init = new HardCodeInitializer();

            var start = init.SimuStartDate;
            var step = init.RebalancingStep;
            var window = init.EstimationWindow;
            var vanillaOpt = init.initVanillaOpt();

            var res = vanillaOpt.GenChartData(window, start, step, marketSimulator);

            prettyPrintRes(vanillaOpt, res, window);
        }

        [TestMethod()]
        public void BasketTest()
        {
            var marketSimulator = new SimulatedDataFeedProvider();
            //var marketSimulator = new HistDataFeedProvider();

            var init = new HardCodeInitializer();

            var start = init.SimuStartDate;
            var step = init.RebalancingStep;
            var window = init.EstimationWindow;
            var basketOpt = init.initBasketOpt();

            var res = basketOpt.GenChartData(window, start, step, marketSimulator);

            prettyPrintRes(basketOpt, res, window);
        }

        private void prettyPrintRes(FinancialComputation opt, PriceOpValPort res, int window)
        {
            Console.WriteLine("Demarrer \n");
            Console.WriteLine("Financial product: " + opt.Option.Name + "\n");
            for (int date = 0; date < res.OptionPrice.Count; date++)
            {
                Console.WriteLine("Date: " + opt.MarketDataDates[window + date]);
                Console.WriteLine("Option price: " + res.OptionPrice[date]);
                Console.WriteLine("Portfolio value:" + res.PortfolioValue[date].Value);
                var trackErr = res.OptionPrice[date] - res.PortfolioValue[date].Value;
                Console.WriteLine("Tracking error: " + trackErr);
                for (var shareId = 0; shareId < opt.Option.UnderlyingShareIds.Length; shareId++)
                {
                    Console.WriteLine("   Nb of share of ID '" + opt.Option.UnderlyingShareIds[shareId] + "': " + res.PortfolioValue[date].Deltas[shareId]);
                }
                Console.WriteLine("   Value invested at a free risk rate:" + res.PortfolioValue[date].FreeRiskDelta + "\n");
            }

            Console.WriteLine("At Maturity:");
            Console.WriteLine("Maturity Date: " + opt.Option.Maturity);
            var dic = new Dictionary<string, decimal>();
            for (var shareId = 0; shareId < opt.Option.UnderlyingShareIds.Length; shareId++)
            {
                dic.Add(opt.Option.UnderlyingShareIds[shareId], (decimal)opt.Spots.Last()[shareId]);
            }
            Console.WriteLine("Payoff: " + opt.Option.GetPayoff(dic) + "\n");
            Console.WriteLine("End");
        }
    }
}