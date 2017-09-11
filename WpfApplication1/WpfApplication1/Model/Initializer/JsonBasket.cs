using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.Model.Initializer
{
    class JsonBasket : IOption
    {
        public DateTime Maturity
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public double[] Weights { get; set; }

        public double Strike
        {
            get; set;
        }

        public string[] UnderlyingShareIds
        {
            get;set;
        }

        public string[] UnderlyingShareNames
        {
            get; set;
        }

        public JsonBasket(string name, string[] underlyingShares, double[] weights, DateTime maturity, double strike, string[] underlyingSharesNames)
        {
            Name = name;
            UnderlyingShareIds = underlyingShares;
            Weights = weights;
            Maturity = maturity;
            Strike = strike;
            UnderlyingShareNames = underlyingSharesNames;
        }

        public BasketOption ToBasket()
        {
            Share[] shares = new Share[UnderlyingShareNames.Count()];
            for (var i =0; i<UnderlyingShareNames.Count(); i++)
            {
                var share = new JsonShare(UnderlyingShareIds[i], UnderlyingShareNames[i]);
                var sh = share.toShare();
                shares[i] = sh;
            }
            

            return new BasketOption(Name, shares, Weights, Maturity, Strike);
        }

        public double GetPayoff(Dictionary<string, decimal> priceList)
        {
            throw new NotImplementedException();
        }
    }
}
