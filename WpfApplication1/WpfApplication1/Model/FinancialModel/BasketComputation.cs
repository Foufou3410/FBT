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

        protected override double[,] ComputeCorrelation(int window, int startingPoint)
        {
            double[,] data = new double[window, Option.UnderlyingShareIds.Length];
            for (int i=0; i<window;i++)
            {
                for(int j=0; j<Option.UnderlyingShareIds.Length; j++)
                {
                    data[i,j] = Spots[startingPoint - window + i][j];
                }
            }
            double[,] correlation = WRE.computeCorrelationMatrix(data);

            return correlation;
        }
        #endregion Protected Methods
    }
}
