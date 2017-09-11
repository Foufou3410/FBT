using AppelWRE;
using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.Model.FinancialModel
{
    public class VanillaComputation : FinancialComputation
    {
        #region Public Properties
        protected VanillaCall Vanilla { get; }
        #endregion Public Properties

        #region Public Constructor
        public VanillaComputation(VanillaCall v) : base(v)
        {
            Vanilla = v;
        }
        #endregion

        #region Protected Methods
        override protected PricingResults ComputePricing(int dateIndex, double[] volatility, double[,] correlation)
        {
            var pricer = new Pricer();
            return pricer.PriceCall(Vanilla, MarketDataDates[dateIndex], 365, Spots[dateIndex][0], volatility[0]);
        }

        protected override double[,] ComputeCorrelation(int dateIndex, int startingPoint)
        {
            return new double[,] { { 1 } };
        }
        #endregion Protected Methods
    }
}