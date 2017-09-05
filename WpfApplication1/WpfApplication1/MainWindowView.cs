using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Model.Initializer;

namespace Fbt
{
    class MainWindowView
    {
        private IOption opt;
        private Dictionary<String, decimal> dic;
        private IInitializer init;

        #region Public Constructors

        public MainWindowView()
        {
            Console.WriteLine("Demarrer");
            init = new HardCodeInitializer();
            opt = init.initOptionsUnivers();
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
