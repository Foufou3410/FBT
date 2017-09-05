using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model
{
    class Class1
    {
    }
}







using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fbt
{
    class MainWindowView
    {
        public MainWindowView()
        {
            Console.WriteLine("Demarrer");
            var date = new DateTime(2018, 6, 24);
            var sh = new PricingLibrary.FinancialProducts.Share("BNP", "1");
            //var sh2 = new PricingLibrary.FinancialProducts.Share("CA", "2");
            var opt = new PricingLibrary.FinancialProducts.VanillaCall("option", sh, date, 27);
            //var dic = new Dictionary<String, decimal>() { {"1", 38.1m }};
            var pricer = new PricingLibrary.Computations.Pricer();
            var result = pricer.PriceCall(opt, new DateTime(2017, 9, 5), 365, 30, 0.5);

            //var payoff = opt.GetPayoff(dic);


            var dates = new List<DateTime>() { new DateTime(2017, 9, 24), new DateTime(2017, 9, 25), new DateTime(2017, 9, 26), new DateTime(2017, 9, 27) };
            var spots = new List<double>() { 30, 32, 29, 30 };
            var vol = new List<double>() { 0.1, 0.08, 0.09, 0.1 };

            var res = ComputeDeltasAndPrice(dates, opt, spots, vol);
            var port = ComputePricePortefeuille(dates, res, spots, 0.01);
            var j = res.Count;

            for (int i = 0; i < j; i++)
            {
                Console.WriteLine(res[i].price);
                Console.WriteLine(port[i].price);
                //var dic = new Dictionary<String, decimal>() { { "1", (decimal)(spots[i]) } };
                //var payoff = opt.GetPayoff(dic);
                //Console.WriteLine(payoff);
            }


        }

        public class PriceAndDeltas
        {

            public PriceAndDeltas(DateTime d, double p, double[] del)
            {
                date = d;
                price = p;
                deltas = del;
            }
            public DateTime date { get; set; }

            public double price { get; set; }

            public double[] deltas { get; set; }


        }

        public class PricePortefeuille
        {

            public PricePortefeuille(DateTime d, double p)
            {
                date = d;
                price = p;
            }
            public DateTime date { get; set; }

            public double price { get; set; }



        }


        public List<PriceAndDeltas> ComputeDeltasAndPrice(List<DateTime> dates, PricingLibrary.FinancialProducts.VanillaCall option, List<double> spots, List<double> volatilities)
        {
            var result = new List<PriceAndDeltas>();
            var pricer = new PricingLibrary.Computations.Pricer();
            var i = 0;
            foreach (DateTime d in dates)
            {
                var res = pricer.PriceCall(option, d, 365, spots[i], volatilities[i]);
                i++;
                result.Add(new PriceAndDeltas(d, res.Price, res.Deltas));
            }
            return result;

        }


        public List<PricePortefeuille> ComputePricePortefeuille(List<DateTime> dates, List<PriceAndDeltas> deltas, List<double> spots, double tauxSansRisque)
        {
            var result = new List<PricePortefeuille>();
            var i = 0;
            foreach (DateTime d in dates)
            {
                var delta = deltas[i].deltas[0];
                var q2 = (deltas[i].price - delta * spots[i]) / (1 + tauxSansRisque);
                var price = delta * spots[i] + q2 * (1 + tauxSansRisque);

                i++;
                result.Add(new PricePortefeuille(d, price));
            }
            return result;

        }



    }



}