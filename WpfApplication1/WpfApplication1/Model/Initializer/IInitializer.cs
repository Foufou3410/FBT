using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;

namespace WpfApplication1.Model.Initializer
{
    interface IInitializer
    {
        #region Public Methods

        IOption initOptionsUnivers();

        #endregion Public Methods
    }
}
