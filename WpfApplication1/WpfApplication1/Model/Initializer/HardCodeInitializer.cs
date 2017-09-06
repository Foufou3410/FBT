using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;

namespace WpfApplication1.Model.Initializer
{
    class HardCodeInitializer
    {
        public VanillaCall initOptionsUnivers()
        {
            var sousJacent = new Share("BNP", "1");
            var date = new DateTime(2018, 6, 24);
            var opt = new VanillaCall("optionBNP", sousJacent, date, 12);
            return opt;
        }

        public List<DateTime> getDatesOfSimuData()
        {
            return new List<DateTime>() { new DateTime(2017, 9, 24), new DateTime(2017, 9, 25), new DateTime(2017, 9, 26), new DateTime(2017, 9, 27) };
        }

        public List<double> getSpotOfSimuData()
        {
            return new List<double>() { 30, 32, 29, 30 };
        }

        public List<double> getVolatilityOfSimuData()
        {
            return new List<double>() { 0.1, 0.08, 0.09, 0.1 };
        }
    }
}
