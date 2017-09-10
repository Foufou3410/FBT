using System;
using System.Collections.Generic;
using PricingLibrary.FinancialProducts;
using AppelWRE;

namespace FBT.Model.Initializer
{
    public class HardCodeInitializer : IInitializer
    {
        public VanillaCall initVanillaOpt(DateTime debut, double duree)
        {
            var sousJacent = new Share("BNP", "AC FP");
            var date = debut.AddDays(duree);
            var opt = new VanillaCall("optionBNP", sousJacent, date, 8);
            return opt;
        }

        public BasketOption initBasketOpt(DateTime debut, double duree)
        {
            var sousJacent1 = new Share("BNP", "1");
            var sousJacent2 = new Share("AXA", "2");
            var sousJacent3 = new Share("Accenture", "3");
            var date = debut.AddDays(duree);
            var opt = new BasketOption("basketBNP", new Share[]{sousJacent1, sousJacent2, sousJacent3}, new double[] { 0.5, 0.3, 0.2 }, date, 8);
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

        public List<double> computeListVolatility(int window, List<double> spots, int duree)
        {
            var result = new List<double>();
            for (var j = window; j < duree; j++)
            {
                double[,] tab = new double[window, 1];
                for (var k = 1; k < window; k++)
                {
                    tab[k - 1, 0] = Math.Log(spots[j - window + k] / spots[j - window + k - 1]);
                }

                var B = Math.Sqrt(PricingLibrary.Utilities.DayCount.ConvertToDouble(1, 365));

                double[,] myVol = WRE.computeVolatility(tab);
                result.Add(myVol[0, 0] / B);
            }

            return result;

        }
    }
}
