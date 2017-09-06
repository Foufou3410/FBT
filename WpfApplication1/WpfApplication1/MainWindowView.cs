using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Model.FinancialModel;
using WpfApplication1.Model.Initializer;

namespace Fbt
{
    class MainWindowView
    {

        #region Public Constructors

        public MainWindowView()
        {
            Console.WriteLine("Demarrer");
            var init = new HardCodeInitializer();
            var calculator = new FinancialComputation();
            var opt = init.initOptionsUnivers();
            var dates = init.getDatesOfSimuData();
            var vol = init.getVolatilityOfSimuData();
            var spot = init.getSpotOfSimuData();
            var res = calculator.computeDeltasAndPrice(dates, opt, spot, vol);
            var port = calculator.computePricePortfolio(dates, res, spot, 0.01);
            var j = res.Count;

            for (int i = 0; i < j; i++)
            {
                Console.WriteLine("Date: " + res[i].Date);
                Console.WriteLine("Option price: " + res[i].Price);
                Console.WriteLine("Portfolio price:" + port[i].Price + "\n");
            }
            Console.WriteLine("End");
        }
        #endregion Public Constructors

        /*#region Public Properties

        public string ViewPayOff
        {
           
           get { Console.WriteLine("dans get"); return opt.GetPayoff(dic).ToString(); }
        }

        #endregion Public Properties*/

    }
}
