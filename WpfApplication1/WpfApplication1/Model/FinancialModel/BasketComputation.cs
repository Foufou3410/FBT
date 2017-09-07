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
    public class BasketComputation
    {
        #region Public Properties
        public BasketOption basket { get; }

        public Dictionary<DateTime, double[]> spots { get; }
        #endregion Public Properties

        #region Public Constructor
        public BasketComputation(BasketOption b, DateTime debTest)
        {
            basket = b;
            spots = new Dictionary<DateTime, double[]>();

            var simulateMarket = new SimulatedDataFeedProvider();
            var dataFeed = simulateMarket.GetDataFeed(basket, debTest);
            foreach (DataFeed d in dataFeed)
            {
                var spotslist = new List<double>();
                foreach (KeyValuePair<string, decimal> entry in d.PriceList)
                {
                    spotslist.Add((double)entry.Value);
                }
                spots.Add(d.Date, spotslist.ToArray());
            }
        }
        #endregion

        #region Public Methods
        public List<PriceProdFin> computePrice(List<DateTime> dates)
        {
            var res = new List<PriceProdFin>();
            var pricer = new Pricer();

            foreach (DateTime day in dates)
            {
                //Incrément: volatilities et correlation en dur. A passer en paramètre plus tard
                var volatilities = new List<double>();
                foreach (double s in spots[day])
                {
                    volatilities.Add(0.4);
                }
                double[,] correlation = new double[spots[day].Length, spots[day].Length];
                for (int i = 0; i < spots[day].Length; i++)
                {
                    for (int j = 0; j < spots[day].Length; j++)
                    {
                        if (i != j)
                        {
                            correlation[i, j] = 0.1;
                        }
                        else
                        {
                            correlation[i, j] = 1;
                        }
                    }
                }

                var priceList = pricer.PriceBasket(basket, day, 365, spots[day], volatilities.ToArray(), correlation);
                res.Add(new PriceProdFin(day, priceList.Price));
            }
            return res;
        }

        public List<PricePortfolio> computeValuePortfolio(List<DateTime> dates, List<DateTime> rebalancingDates, double riskFreeRate)
        //The first date of dates and of rebalancingDates must be the same
        {
            var res = new List<PricePortfolio>();
            var deltas = computeRebalPriceAndDeltas(rebalancingDates);
            var i = 0;
            var valPortefeuille = deltas[dates[i]].Price;

            var currentDelta = deltas[dates[i]].Deltas;
            var previousDelta = currentDelta;

            res.Add(new PricePortfolio(dates[i], valPortefeuille, listProd(deltas[dates[i]].Deltas, spots[dates[i]]), valPortefeuille - listProd(deltas[dates[i]].Deltas, spots[dates[i]]).Sum()));

            for (i = 1; i < dates.Count; i++)
            {
                if (deltas.ContainsKey(dates[i]))
                {
                    var tmp = currentDelta;
                    currentDelta = deltas[dates[i]].Deltas;
                    previousDelta = tmp;
                }
                valPortefeuille = listProd(currentDelta, spots[dates[i]]).Sum() + listProd(previousDelta, spots[dates[i]]).Sum() + (valPortefeuille - listProd(previousDelta, spots[dates[i - 1]]).Sum()) * (riskFreeRate) - listProd(currentDelta, spots[dates[i]]).Sum();
                res.Add(new PricePortfolio(dates[i], valPortefeuille, listProd(currentDelta, spots[dates[i]]), valPortefeuille - listProd(currentDelta, spots[dates[i]]).Sum()));
            }
            return res;
        }

        #endregion

        #region Private Methods
        private Dictionary<DateTime, PriceAndDelta> computeRebalPriceAndDeltas(List<DateTime> rebalancingDates)
        {
            var res = new Dictionary<DateTime, PriceAndDelta>();
            var pricer = new Pricer();

            foreach (DateTime day in rebalancingDates)
            {
                //Incrément: volatilities et correlation en dur. A passer en paramètre plus tard
                var volatilities = new List<double>();
                foreach (double s in spots[day])
                {
                    volatilities.Add(0.4);
                }
                double[,] correlation = new double[spots[day].Length, spots[day].Length];
                for (int i = 0; i < spots[day].Length; i++)
                {
                    for (int j = 0; j < spots[day].Length; j++)
                    {
                        if (i != j)
                        {
                            correlation[i, j] = 0.1;
                        }
                        else
                        {
                            correlation[i, j] = 1;
                        }
                    }
                }

                var priceList = pricer.PriceBasket(basket, day, 365, spots[day], volatilities.ToArray(), correlation);
                res.Add(day, new PriceAndDelta(day, priceList.Price, priceList.Deltas));
            }
            return res;
        }

        private List<double> listProd (double[] a, double[] b)
        //a et b doivent être de même taille
        {
            var res = new List<double>();
            for (int i = 0; i < a.Length; i++)
            {
                res.Add (a[i] * b[i]);
            }
            return res;
        }
        #endregion Private Methods
    }
}
