using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppelWRE;
using System.Diagnostics;
using System.Runtime.InteropServices;

using FBT.Model.FinancialModel;
using FBT.Model.Initializer;

namespace FBT
{
    class MainWindowView
    {

        #region Public Constructors

        public MainWindowView()
        {

            Console.WriteLine("Démarrer");

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


