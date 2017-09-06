using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model.FinancialModel
{
    class DataFeed
    {
        public void GenerateDataFeed()
        {
            var sousJacent = new Share("BNP", "1");
            var date = new DateTime(2017, 9, 30);
            var opt = new VanillaCall("optionBNP", sousJacent, date, 33);
            var simulateMarket = new PricingLibrary.Utilities.MarketDataFeed.SimulatedDataFeedProvider();
            var datafeed = simulateMarket.GetDataFeed(opt, new DateTime(2017, 9, 24));
        }
    }
}
