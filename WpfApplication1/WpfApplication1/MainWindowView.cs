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
            var date = new DateTime(24/06/2018);
            var sh = new PricingLibrary.FinancialProducts.Share("BNP", "1");
            //var sh2 = new PricingLibrary.FinancialProducts.Share("CA", "2");
            var opt = new PricingLibrary.FinancialProducts.VanillaCall("option", sh, date, 12);
            var dic = new Dictionary<String, decimal>() { {"1", 38.1m }};
            var payoff = opt.GetPayoff(dic);
            Console.WriteLine(payoff);
        }
    }
}
