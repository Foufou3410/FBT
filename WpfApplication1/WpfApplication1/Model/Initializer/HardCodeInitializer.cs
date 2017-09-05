using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;

namespace WpfApplication1.Model.Initializer
{
    class HardCodeInitializer : IInitializer
    {
        Share sousJacent;

        public HardCodeInitializer()
        {
            sousJacent = new Share("BNP", "1");
        }
        public IOption initOptionsUnivers()
        {
            var date = new DateTime(24 / 06 / 2018);
            var opt = new VanillaCall("optionBNP", sousJacent, date, 12);
            return opt;
        }
    }
}
