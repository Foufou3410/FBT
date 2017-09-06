using System;
using System.Collections.Generic;
using PricingLibrary.FinancialProducts;

namespace FBT.Model.Initializer
{
    public class HardCodeInitializer : IInitializer
    {
        public VanillaCall initOptionsUnivers(DateTime debut, double duree)
        {
            var sousJacent = new Share("BNP", "1");
            var date = debut.AddDays(duree);
            var opt = new VanillaCall("optionBNP", sousJacent, date, 8);
            return opt;
        }

        public List<DateTime> getRebalancingDates(DateTime debut, double duree, int pas)
        {
            var dates = new List<DateTime>();
            var current = debut;
            dates.Add(debut);
            for (var i=1; i < duree/pas; i++)
            {
                current = current.AddDays(pas);
                dates.Add(current);
            }
            return dates;
        }


        public List<double> getVolatilityOfSimuData(double duree, int pas)
        {
            var vol = new List<double>();
            for (var i = 0; i < duree/pas; i++)
            {
                vol.Add(0.4);
            }
            return vol;
        }

        public double initRiskFreeRate(int pas)
        {
            var span = PricingLibrary.Utilities.DayCount.ConvertToDouble(pas, 365);
            var free = PricingLibrary.Utilities.MarketDataFeed.RiskFreeRateProvider.GetRiskFreeRateAccruedValue(span);
            return free;
        }
    }
}
