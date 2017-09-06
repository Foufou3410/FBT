using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppelWRE;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

            var riskFreeRate = init.initRiskFreeRate();
            var opt = init.initOptionsUnivers();
            var dates = init.getDatesOfSimuData();
            var vol = init.getVolatilityOfSimuData();
            var spot = init.getSpotOfSimuData();

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


            //public void WREmodelingCovTest()
            //      {
            // header
            Console.WriteLine("******************************");
            Console.WriteLine("*    WREmodelingCov in C#   *");
            Console.WriteLine("******************************");

            // sample data
            double[,] returns = { {0.05, 0.05, 0.6}, {-0.001, -0.001, 0.56}, {0.7, 0.7, 0.12}, {-0.3, -0.3, -0.1},
                                {0.1, 0.1, 0.3}};
            
            // call WRE via computeCovarianceMatrix encapsulation
            //WRE classWRE = new WRE();
            double[,] myCovMatrix = WRE.computeCovarianceMatrix(returns);

            // display result
            WRE.dispMatrix(myCovMatrix);
                   
            // ending the program            
            Console.WriteLine("\nType enter to exit");
            //Console.ReadKey(true);
            // }



        }
        #endregion Public Constructors

        /*#region Public Properties

        public string ViewPayOff
        {
           
           get {return opt.GetPayoff(dic).ToString(); }
        }

        #endregion Public Properties*/

    }

}


