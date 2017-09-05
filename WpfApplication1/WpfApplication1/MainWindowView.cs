using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fbt
{
    class MainWindowView
    {
        private DateTime date;
        private PricingLibrary.FinancialProducts.Share sh;
        private PricingLibrary.FinancialProducts.VanillaCall opt;
        private Dictionary<String, decimal> dic;

        #region Public Constructors

        public MainWindowView()
        {
            Console.WriteLine("Demarrer");
            date = new DateTime(24/06/2018);
            sh = new PricingLibrary.FinancialProducts.Share("BNP", "1");
            //var sh2 = new PricingLibrary.FinancialProducts.Share("CA", "2");
            opt = new PricingLibrary.FinancialProducts.VanillaCall("option", sh, date, 12);
            dic = new Dictionary<String, decimal>() { {"1", 38.1m }};
        }
        #endregion Public Constructors

        #region Public Properties

        public string ViewPayOff
        {
           
           get { Console.WriteLine("dans get"); return opt.GetPayoff(dic).ToString(); }
        }

        #endregion Public Properties

    }
}
