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
        public VanillaCall vanilla { get; }

        public Dictionary<DateTime, double> spots { get; }

        public Dictionary<DateTime, double> volatility { get; }

        #region Public Constructor
        public VanillaComputation(VanillaCall v, DateTime debTest, int window)
        {
            vanilla = v;
            spots = new Dictionary<DateTime, double>();
            var spotsList = new List<double>();
            var dateList = new List<DateTime>();

            var simulateMarket = new SimulatedDataFeedProvider();
            var dataFeed = simulateMarket.GetDataFeed(vanilla, debTest);
            foreach (DataFeed d in dataFeed)
            {
                spots.Add(d.Date, (double)d.PriceList[vanilla.UnderlyingShare.Id]);
                spotsList.Add((double)d.PriceList[vanilla.UnderlyingShare.Id]);
                dateList.Add(d.Date);
            }

            volatility = computeListVolatility(window, spotsList, dateList);
        }
        #endregion

        #region Public Methods

        public Dictionary<DateTime, double> computeListVolatility(int window, List<double> spotsList, List<DateTime> dates)
        {
            var result = new Dictionary<DateTime, double>();

            for (var currentDate = window; currentDate < dates.Count; currentDate++)
            {
                double[,] tab = new double[window - 1, 1];
                for (var k = 1; k < window; k++)
                {
                    tab[k - 1, 0] = Math.Log(spotsList[currentDate - window + k] / spotsList[currentDate - window + k - 1]);
                }

                var B = Math.Sqrt(PricingLibrary.Utilities.DayCount.ConvertToDouble(1, 365));
                double[,] myVol = WRE.computeVolatility(tab);
                result.Add(dates[currentDate], myVol[0, 0] / B);
            }
            return result;
        }

        public List<PriceProdFin> computePrice (List<DateTime> dates)
        //TODO: renvoyer une erreur si le spot est vide
        {
            var res = new List<PriceProdFin>();
            var pricer = new Pricer();

            foreach (DateTime day in dates)
            {
                var priceList = pricer.PriceCall(vanilla, day, 365, spots[day], volatility[day]);
                res.Add(new PriceProdFin(day, priceList.Price));
            }
            return res;
        }

        public List<PricePortfolio> computeValuePortfolio(List<DateTime> dates, List<DateTime> rebalancingDates,  double riskFreeRate)
        //The first date of dates and of rebalancingDates must be the same
        //TODO: renvoyer une erreur si le spot est vide
        {
            var res = new List<PricePortfolio>();
            var deltas = computeRebalPriceAndDeltas(rebalancingDates);
            var i = 0;
            var valPortefeuille = deltas[dates[i]].Price;

            var currentDelta = deltas[dates[i]].Deltas[0];
            var previousDelta = currentDelta;

            res.Add(new PricePortfolio(dates[i], valPortefeuille, new List<double> { deltas[dates[i]].Deltas[0] * spots[dates[i]] }, valPortefeuille - deltas[dates[i]].Deltas[0] * spots[dates[i]]));

            for (i = 1; i < dates.Count; i++)
            {
                if (deltas.ContainsKey(dates[i]))
                {
                    var tmp = currentDelta;
                    currentDelta = deltas[dates[i]].Deltas[0];
                    previousDelta = tmp;
                }
                valPortefeuille = currentDelta * spots[dates[i]] + (previousDelta * spots[dates[i]] + (valPortefeuille - previousDelta * spots[dates[i-1]]) * (riskFreeRate) - currentDelta * spots[dates[i]]);
                res.Add(new PricePortfolio(dates[i], valPortefeuille, new List<double> { currentDelta * spots[dates[i]] }, valPortefeuille - currentDelta * spots[dates[i]]));
            }
            return res;
        }

        #endregion

        #region Private Methods
        private Dictionary<DateTime, PriceAndDelta> computeRebalPriceAndDeltas(List<DateTime> rebalancingDates)
        //TODO: renvoyer une erreur si le spot est vide
        {
            var res = new Dictionary<DateTime, PriceAndDelta>();
            var pricer = new Pricer();

            foreach (DateTime day in rebalancingDates)
            {
                var priceList = pricer.PriceCall(vanilla, day, 365, spots[day], volatility[day]);
                res.Add(day, new PriceAndDelta(day, priceList.Price, priceList.Deltas));
            }
            return res;
        }
        #endregion Private Methods
    }
}
