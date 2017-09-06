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
            var init = new HardCodeInitializer();
            var calculator = new FinancialComputation();


            var debut = new DateTime(2017, 9, 6);
            var duree = 365;
            var pas = 2;

            var opt = init.initOptionsUnivers(debut, duree-1);
            var dataFeedProvider = new MyDataFeed();
            var spot = dataFeedProvider.GenerateDataFeed(debut, duree-1, opt);

            

            var riskFreeRate = init.initRiskFreeRate(pas);
            
            var dates = init.getDatesOfSimuData(debut, duree-1, pas);
            var vol = init.getVolatilityOfSimuData(duree-1, pas);

            Console.WriteLine(dates.Count);
            Console.WriteLine(vol.Count);
            Console.WriteLine(spot.Count);

            var res = calculator.computeDeltasAndPrice(dates, opt, spot, vol, pas);
            var port = calculator.computePricePortfolio(dates, res, spot, riskFreeRate, pas);
            var j = res.Count;
            


            Console.WriteLine("Demarrer \n");
            Console.WriteLine("Financial product: " + opt.Name + "\n");
            for (int i = 0; i < j; i++)
            {
                Console.WriteLine("Date: " + res[i].Date);
                Console.WriteLine("Option price: " + res[i].Price);
                Console.WriteLine("Portfolio value:" + port[i].Price);
                var trackErr = res[i].Price - port[i].Price;
                Console.WriteLine("Tracking error: " + trackErr);
                Console.WriteLine("Composition of the portfolio: ");
                Console.WriteLine("   Value in the underlying share " + opt.UnderlyingShare.Name + ": " + port[i].valShare);
                Console.WriteLine("   Value invested at a free risk rate:" + port[i].valSansRisque + "\n");
            }
            Console.WriteLine("At Maturity:");
            Console.WriteLine("Maturity Date: " + opt.Maturity);
            var dic = new Dictionary<string, decimal>();
            dic.Add(opt.UnderlyingShare.Id, (decimal)spot[j]);
            Console.WriteLine("Payoff: " + opt.GetPayoff(dic) +"\n");
            Console.WriteLine("End");


            




        }
        #endregion Public Constructors

        /*#region Public Properties

        public string ViewPayOff
        {
           
           get { return opt.GetPayoff(dic).ToString(); }
        }

        #endregion Public Properties*/

    }
}
