using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;

namespace FBT
{
    class MainWindowView
    {

        #region Public Constructors

        public MainWindowView()
        {
            var init = new HardCodeInitializerVanilla();
            var calculator = new FinancialComputation();


            var debut = new DateTime(2017, 9, 6);
            var duree = 365;


            var dataFeedProvider = new MyDataFeed();
            var spot = dataFeedProvider.GenerateDataFeed(debut, duree);

            

            var riskFreeRate = init.initRiskFreeRate();
            var opt = init.initOptionsUnivers(debut, duree);
            var dates = init.getDatesOfSimuData(debut, duree);
            var vol = init.getVolatilityOfSimuData(duree);

            Console.WriteLine(dates.Count);
            Console.WriteLine(vol.Count);
            Console.WriteLine(spot.Count);

            var res = calculator.computeDeltasAndPrice(dates, opt, spot, vol);
            var port = calculator.computePricePortfolio(dates, res, spot, riskFreeRate);
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
            dic.Add(opt.UnderlyingShare.Id, init.getSpotAtMaturity());
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
