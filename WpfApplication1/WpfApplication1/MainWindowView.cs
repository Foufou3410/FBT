using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppelWRE;
using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using FBT.Model.FinancialModel;
using FBT.Model.Initializer;

namespace FBT
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
            double[,] myCovMatrix = WRE.computeCovarianceMatrix(returns);

            // display result
            WRE.dispMatrix(myCovMatrix);

            //Console.WriteLine();


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


