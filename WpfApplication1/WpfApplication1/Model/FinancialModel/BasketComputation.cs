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
    public class BasketComputation : FinancialComputation
    {
        #region Public Properties
        public BasketOption basket { get; }
        #endregion Public Properties

        #region Public Constructor
        public BasketComputation(BasketOption b) : base(b)
        {
            basket = b;
        }
        #endregion

        #region Protected Methods
        protected override PricingResults ComputePricing(int dateIndex, double[] volatility, double[,] correlation)
        {
            var pricer = new Pricer();
            return pricer.PriceBasket(basket, MarketDataDates[dateIndex], 365, Spots[dateIndex], volatility, correlation);
        }

        protected override double[,] ComputeCorrelation(int dateIndex)
        {
            double[,] correlation = new double[Spots[dateIndex].Length, Spots[dateIndex].Length];
            for (int i = 0; i < Spots[dateIndex].Length; i++)
            {
                for (int j = 0; j < Spots[dateIndex].Length; j++)
                {
                    if (i != j)
                    {
                        correlation[i, j] = 0.1;
                    }
                    else
                    {
                        correlation[i, j] = 1;
                    }
                }
            }
            return correlation;
        }
        #endregion Protected Methods
    }
}
