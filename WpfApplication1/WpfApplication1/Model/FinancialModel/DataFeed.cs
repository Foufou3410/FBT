using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBT.Model.Initializer;

namespace FBT.Model.FinancialModel
{
    class MyDataFeed
    {
        public List<double> GenerateDataFeed(DateTime debut, double duree)
        {

            //var init = new HardCodeInitializer();
            var sousJacent = new Share("BNP", "1");
            var date = debut.AddDays(duree);
            var opt = new VanillaCall("optionBNP", sousJacent, date, 33);
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
