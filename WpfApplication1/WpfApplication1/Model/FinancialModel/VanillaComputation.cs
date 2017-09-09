using AppelWRE;
using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.Model.FinancialModel
{
    public class VanillaComputation
    {
        public class PriceOpValPort
        {
            public List<Portfolio> PortfolioValue { get; }

            public List<double> OptionPrice { get; }

            public PriceOpValPort (List<Portfolio> valP, List<double> priceOp)
            {
                PortfolioValue = valP;
                OptionPrice = priceOp;
            }
        }

        public VanillaCall Vanilla { get; }

        public List<double> Spots { get; }

        public List<DateTime> MarketDataDates { get; }

        #region Public Constructor
        public VanillaComputation(VanillaCall v)
        {
            Vanilla = v;
            Spots = new List<double>();
            MarketDataDates = new List<DateTime>();
        }
        #endregion

        #region Public Methods

        public PriceOpValPort GenChartData(int estimationWindow, DateTime debTest, int rebalancingStep, IDataFeedProvider simulateMarket)
        {
            getSpots(debTest, simulateMarket);

            var pricer = new Pricer();
            var priceOpt = new List<double>();
            var valPort = new List<Portfolio>();

            //The first datafeed considered is the one at date window
            var volatility = ComputeVolatility(estimationWindow, estimationWindow, rebalancingStep);
            var initialSpot = Spots[estimationWindow];
            var priceList = pricer.PriceCall(Vanilla, MarketDataDates[estimationWindow], 365, initialSpot, volatility);

            var valPortFolio = priceList.Price;
            var deltas = priceList.Deltas;
            var freeRiskQuantity = priceList.Price - priceList.Deltas[0] * initialSpot;           
            var consideredPortfolio = new Portfolio(valPortFolio, deltas, freeRiskQuantity);

            priceOpt.Add(priceList.Price);
            valPort.Add(new Portfolio (consideredPortfolio));

            for (var i = estimationWindow + 1; i < Spots.Count; i++)
            {//For each data feed except the first one        
                priceList = pricer.PriceCall(Vanilla, MarketDataDates[i], 365, Spots[i], volatility);
                consideredPortfolio.updateValue(new double[] { Spots[i] });

                if ((i - estimationWindow)%rebalancingStep == 0)
                {//if there is a rebalancing
                    volatility = ComputeVolatility(estimationWindow, i, rebalancingStep);
                    consideredPortfolio.Deltas = priceList.Deltas;
                }

                consideredPortfolio.updateFreeRiskDelta(new double[] { Spots[i] });

                priceOpt.Add(priceList.Price);
                valPort.Add(new Portfolio (consideredPortfolio));
            }

            return new PriceOpValPort(valPort, priceOpt);
        }

        #endregion

        #region Private Methods
        private double ComputeVolatility(int window, int deb, int step)
        {//Compute the volatility at date deb. Estimate the parameters thanks to the data of the window days before deb
            /*double[,] tab = new double[window - 1, 1];
            for (var k = 1; k < window; k++)
            {
                tab[k - 1, 0] = Math.Log(Spots[deb - window + k] / Spots[deb - window + k - 1]);
            }
            var B = Math.Sqrt(PricingLibrary.Utilities.DayCount.ConvertToDouble(step, 365));
            double[,] myVol = WRE.computeVolatility(tab);
            return myVol[0, 0] / B;*/
            return 0.4;
        }

        private void getSpots(DateTime debTest, IDataFeedProvider simulateMarket)
        {//Get all the spots of the underlying asset from the debTest date to the maturity date
            Spots.Clear();
            var dataFeed = simulateMarket.GetDataFeed(Vanilla, debTest);
            foreach (DataFeed d in dataFeed)
            {
                Spots.Add((double)d.PriceList[Vanilla.UnderlyingShare.Id]);
                MarketDataDates.Add(d.Date);
            }
        }
        #endregion Private Methods
    }
}