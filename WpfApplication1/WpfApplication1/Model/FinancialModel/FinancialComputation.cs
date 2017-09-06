using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model.FinancialModel
{
    class FinancialComputation
    {
        public List<PriceAndDelta> computeDeltasAndPrice(List<DateTime> dates, VanillaCall option, List<double> spots, List<double> volatilities)
        {
            var result = new List<PriceAndDelta>();
            var pricer = new Pricer();
            var i = 0;
            foreach (DateTime d in dates)
            {
                var res = pricer.PriceCall(option, d, 365, spots[i], volatilities[i]);
                i++;
                result.Add(new PriceAndDelta(d, res.Price, res.Deltas));
            }
            return result;
        }

        public List<PricePortfolio> computePricePortfolio(List<DateTime> dates, List<PriceAndDelta> deltas, List<double> spots, double tauxSansRisque)
        {
            var result = new List<PricePortfolio>();
            var i = 0;
            foreach (DateTime d in dates)
            {
                var delta = deltas[i].Deltas[0];
                var q2 = (deltas[i].Price - delta * spots[i]) / (1 + tauxSansRisque);
                var price = delta * spots[i] + q2 * (1 + tauxSansRisque);

                i++;
                result.Add(new PricePortfolio(d, price));
            }
            return result;
        }
    }
}