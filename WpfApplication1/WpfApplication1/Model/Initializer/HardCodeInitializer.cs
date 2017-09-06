using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;

namespace FBT.Model.Initializer
{
    class HardCodeInitializer
    {
        public VanillaCall initOptionsUnivers(DateTime debut, double duree)
        {
            var sousJacent = new Share("BNP", "1");
            var date = debut.AddDays(duree);
            var opt = new VanillaCall("optionBNP", sousJacent, date, 8);
            return opt;
        }

        public List<DateTime> getDatesOfSimuData(DateTime debut, double duree)
        {
            var dates = new List<DateTime>();
            var current = debut;
            dates.Add(debut);
            for (var i=1; i < duree; i++)
            {
                current = current.AddDays(1);
                dates.Add(current);
            }
            return dates;
        }

        public decimal getSpotAtMaturity()
        {
            return 35m;
        }

        /*public List<double> getSpotOfSimuData()
        {
            return new List<double>() { 30, 32, 29, 30 };
        }*/

        public List<double> getVolatilityOfSimuData(double duree)
        {
            var vol = new List<double>();
            for (var i = 0; i < duree; i++)
            {
                vol.Add(0.4);
            }
            return vol;
        }

        public double initRiskFreeRate()
        {
            var span = PricingLibrary.Utilities.DayCount.ConvertToDouble(1, 365);
            var free = PricingLibrary.Utilities.MarketDataFeed.RiskFreeRateProvider.GetRiskFreeRateAccruedValue(span);
            return free;
        }
    }
}
