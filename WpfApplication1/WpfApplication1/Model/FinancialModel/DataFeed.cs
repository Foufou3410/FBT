using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBT.Model.Initializer;

namespace FBT.Model.FinancialModel
{
    public class MyDataFeed
    {
        public List<double> GenerateDataFeed(DateTime debut, IOption opt)
        {
            var simulateMarket = new PricingLibrary.Utilities.MarketDataFeed.SimulatedDataFeedProvider();
            var datafeed = simulateMarket.GetDataFeed(opt, debut);
            var spots = new List<double>();

            foreach (PricingLibrary.Utilities.MarketDataFeed.DataFeed d in datafeed)
            {
                foreach (KeyValuePair<string, decimal> di in d.PriceList)
                {
                    spots.Add((double)di.Value);
                }
            }

            return spots;
        }
    }
}
