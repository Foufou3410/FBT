using AppelWRE;
using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FBT.Model.FinancialModel
{
    public abstract class FinancialComputation
    {
        #region Public Properties
        public string Name { get { return Option.Name; } }

        public IOption Option { get; }

        public List<double[]> Spots { get; }

        public List<DateTime> MarketDataDates { get; }

        public double PayOff { get; private set; } 
        #endregion Public Properties

        #region Public Constructor
        public FinancialComputation (IOption opt)
        {
            Option = opt;
            Spots = new List<double[]>();
            MarketDataDates = new List<DateTime>();
        }
        #endregion Public Constructor

        #region Public Methods
        public PriceOpValPort GenChartData(int estimationWindow, DateTime beginningTest, int rebalancingStep, IDataFeedProvider simulateMarket)
        {
            GetSpots(beginningTest, simulateMarket);

            var priceOpt = new List<double>();
            var valPort = new List<Portfolio>();

            //The first datafeed considered is the one at date window
            var volatility = ComputeVolatility(estimationWindow, estimationWindow, rebalancingStep);
            var correlation = ComputeCorrelation(estimationWindow, estimationWindow);
            var pricingRes = ComputePricing(estimationWindow, volatility, correlation);

            var valPortFolio = pricingRes.Price;
            var deltas = pricingRes.Deltas;
            var initialSpots = Spots[estimationWindow];
            var consideredPortfolio = new Portfolio(valPortFolio, deltas, initialSpots);

            priceOpt.Add(pricingRes.Price);
            valPort.Add(new Portfolio(consideredPortfolio));

            for (var i = estimationWindow + 1; i < Spots.Count; i++)
            {//For each data feed except the first one
                pricingRes = ComputePricing(i, volatility, correlation);
                consideredPortfolio.UpdateValue(Spots[i]);

                if ((i - estimationWindow) % rebalancingStep == 0)
                {//if there is a rebalancing
                    volatility = ComputeVolatility(estimationWindow, i, rebalancingStep);
                    correlation = ComputeCorrelation(estimationWindow , i);
                    consideredPortfolio.Deltas = pricingRes.Deltas;
                }

                consideredPortfolio.UpdateFreeRiskDelta(Spots[i]);

                priceOpt.Add(pricingRes.Price);
                valPort.Add(new Portfolio(consideredPortfolio));
            }

            return new PriceOpValPort(valPort, priceOpt);
        }
        #endregion Public Methods

        #region Protected Methods
        abstract protected PricingResults ComputePricing(int dateIndex, double[] volatility, double[,] correlation);
        abstract protected double[,] ComputeCorrelation(int estimationWindow, int startPoint);
        #endregion Protected Methods

        #region Private Methods
        private double[] ComputeVolatility(int window, int deb, int step)
        {//Compute the volatility of all the underlying shares at date deb. 
         //Estimate the parameters thanks to the data of the window days before deb.
            var res = new List<double>();
            for (var share = 0; share < Option.UnderlyingShareIds.Length; share ++)
            {
                double[,] tab = new double[window - 1, 1];
                for (var k = 1; k < window; k++)
                {
                    tab[k - 1, 0] = Math.Log((double)Spots[deb - window + k][share] / (double)Spots[deb - window + k - 1][share]);
                }
                var B = Math.Sqrt(DayCount.ConvertToDouble(step, 365));
                double[,] myVol = WRE.computeVolatility(tab);
                res.Add(myVol[0, 0] / B);
            }
            return res.ToArray();
        }

        private void GetSpots(DateTime beginningTest, IDataFeedProvider simulateMarket)
        {//Get all the spots of the underlying asset from the debTest date to the maturity date
            Spots.Clear();
            var firstDateMarket = simulateMarket.GetMinDate();
            if (beginningTest.Date < firstDateMarket.Date)
            {
                throw new Exception("No Market data before the " + firstDateMarket.ToShortDateString());
            }
            if (beginningTest.Date > Option.Maturity.Date)
            {
                throw new Exception("No Market data after the Maturity date (" + Option.Maturity.ToShortDateString() + ")");
            }

            var dataFeed = simulateMarket.GetDataFeed(Option, beginningTest);
            if (dataFeed.Count() == 0)
            {
                throw new Exception("No Market data");
            }
            foreach (DataFeed d in dataFeed)
            {
                var spotList = new List<double>();
                foreach (string shareId in Option.UnderlyingShareIds)
                {
                    spotList.Add((double)d.PriceList[shareId]);
                }
                Spots.Add(spotList.ToArray());
                MarketDataDates.Add(d.Date);
            }
            PayOff = computePayOff();
        }

        private double computePayOff()
        {
            var dic = new Dictionary<string, decimal>();
            for (var shareId = 0; shareId < Option.UnderlyingShareIds.Length; shareId++)
            {
                dic.Add(Option.UnderlyingShareIds[shareId], (decimal)Spots.Last()[shareId]);
            }
            return (Option.GetPayoff(dic));
        }

        #endregion Private Methods
    }
}
