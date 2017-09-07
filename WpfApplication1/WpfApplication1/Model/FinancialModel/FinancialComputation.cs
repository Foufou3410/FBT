﻿using AppelWRE;
using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model.FinancialModel
{
    public class FinancialComputation
    {
        public List<PriceAndDelta> computeDeltasAndPrice(List<DateTime> dates, VanillaCall option, List<double> spots, List<double> volatilities, int pas)
        {
            var result = new List<PriceAndDelta>();
            var pricer = new Pricer();
            var i = 0;
            
            foreach (DateTime d in dates)
            {
                var res = pricer.PriceCall(option, d, 365, spots[i*pas], volatilities[i]);
                i++;
                result.Add(new PriceAndDelta(d, res.Price, res.Deltas));
            }
            return result;
        }

        public List<PricePortfolio> computePricePortfolio(List<DateTime> dates, List<PriceAndDelta> deltas, List<double> spots, double tauxSansRisque, int pas)
        {
            var result = new List<PricePortfolio>();
            var i = 1;
            var valPortefeuille = deltas[0].Price;
            result.Add(new PricePortfolio(dates[i], valPortefeuille, deltas[0].Deltas[0]*spots[0], valPortefeuille- deltas[0].Deltas[0] * spots[0]));
            for (i=1; i<dates.Count; i++)
            {
                valPortefeuille = deltas[i].Deltas[0] * spots[i*pas] + (deltas[i - 1].Deltas[0] * spots[i*pas] + (valPortefeuille - deltas[i - 1].Deltas[0] * spots[(i - 1)*pas]) * (tauxSansRisque) - deltas[i].Deltas[0] * spots[i*pas]);
                result.Add(new PricePortfolio(dates[i], valPortefeuille, deltas[i].Deltas[0] * spots[i*pas], valPortefeuille - deltas[i].Deltas[0] * spots[i*pas]));

            }
            return result;
        }


        public List<double> computeListVolatility(int window, List<double> spots, int duree)
        {


            var result = new List<double>();



            for (var j=window; j<duree; j++) {
                double[,] tab = new double[window, 1];

                for (var k = 1; k < window; k++)
                {
                    tab[k - 1, 0] = Math.Log(spots[j-window+k] / spots[j -window + k - 1]);
                }


                var B = Math.Sqrt(PricingLibrary.Utilities.DayCount.ConvertToDouble(1, 365));


                double[,] myVol = WRE.computeVolatility(tab);
                result.Add(myVol[0, 0]/B);
            }

            return result;

        }






    }
}