using System.Collections.Generic;

namespace FBT.Model.FinancialModel
{
    public class PriceOpValPort
    {
        public List<Portfolio> PortfolioValue { get; }

        public List<double> OptionPrice { get; }

        public PriceOpValPort(List<Portfolio> valP, List<double> priceOp)
        {
            PortfolioValue = valP;
            OptionPrice = priceOp;
        }
    }
}
