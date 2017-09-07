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
        public List<PriceAndDelta> computeDeltasAndPrice(List<DateTime> dates, VanillaCall option, List<double> spots, List<double> volatilities, uint pas)
        {
            var result = new List<PriceAndDelta>();
            var pricer = new Pricer();
            var i = 0;
            
            foreach (DateTime d in dates)
            {
                var res = pricer.PriceCall(option, d, 365, spots[i * (int)pas], volatilities[i]);
                i++;
                result.Add(new PriceAndDelta(d, res.Price, res.Deltas));
            }
            return result;
        }

        public List<PricePortfolio> computePricePortfolio(List<DateTime> dates, List<PriceAndDelta> deltas, List<double> spots, double tauxSansRisque, uint pas)
        {
            var result = new List<PricePortfolio>();
            var i = 1;
            int step = (int)pas;
            var valPortefeuille = deltas[0].Price;
            result.Add(new PricePortfolio(dates[i], valPortefeuille, deltas[0].Deltas[0]*spots[0], valPortefeuille- deltas[0].Deltas[0] * spots[0]));
            for (i=1; i<dates.Count; i++)
            {
                valPortefeuille = deltas[i].Deltas[0] * spots[i*step] + (deltas[i - 1].Deltas[0] * spots[i*step] + (valPortefeuille - deltas[i - 1].Deltas[0] * spots[(i - 1)*step]) * (tauxSansRisque) - deltas[i].Deltas[0] * spots[i*step]);
                result.Add(new PricePortfolio(dates[i], valPortefeuille, deltas[i].Deltas[0] * spots[i*step], valPortefeuille - deltas[i].Deltas[0] * spots[i*step]));

            }
            return result;
        }
    }
}