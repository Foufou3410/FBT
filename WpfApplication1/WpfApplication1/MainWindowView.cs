using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppelWRE;
using System.Diagnostics;
using System.Runtime.InteropServices;



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


//public void WREmodelingCovTest()
  //      {
            // header
            Debug.WriteLine("******************************");
            Debug.WriteLine("*    WREmodelingCov in C#   *");
            Debug.WriteLine("******************************");

            // sample data
            double[,] returns = { {0.05, -0.1, 0.6}, {-0.001, -0.4, 0.56}, {0.7, 0.001, 0.12}, {-0.3, 0.2, -0.1},
                                {0.1, 0.2, 0.3}};

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

        #region Public Properties

        public string ViewPayOff
        {
           
           get {return opt.GetPayoff(dic).ToString(); }
        }

        #endregion Public Properties

    }

}


