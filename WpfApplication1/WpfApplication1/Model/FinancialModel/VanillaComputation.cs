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

        #region Public Constructor
        public VanillaComputation(VanillaCall v, DateTime debTest)
        {
            vanilla = v;
            spots = new Dictionary<DateTime, double>();

            generateDataFeed(debTest, true);
        }
        #endregion

        #region Public Methods
        public void generateDataFeed(DateTime debTest, bool isSimulated)
        {
            if (isSimulated)
            //Case simulated data
            {
                var simulateMarket = new SimulatedDataFeedProvider();
                var dataFeed = simulateMarket.GetDataFeed(vanilla, debTest);
                foreach (DataFeed d in dataFeed)
                {
                    spots.Add(d.Date, (double)d.PriceList[vanilla.UnderlyingShare.Id]);
                }
            }
            else
            //Case historical data
            {

            }
        }

        public List<PriceProdFin> computePrice (List<DateTime> dates)
        //TODO: renvoyer une erreur si le spot est vide
        {
            var res = new List<PriceProdFin>();
            var pricer = new Pricer();

            foreach (DateTime day in dates)
            {
                var priceList = pricer.PriceCall(vanilla, day, 365, spots[day], 0.4);
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
                var priceList = pricer.PriceCall(vanilla, day, 365, spots[day], 0.4);
                res.Add(day, new PriceAndDelta(day, priceList.Price, priceList.Deltas));
            }
            return res;
        }
        #endregion Private Methods
    }
}
