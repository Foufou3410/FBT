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

        #region Public Properties

        public string ViewPayOff
        {
           
           get {return "coucou"; }
        }

        #endregion Public Properties

    }

}


