using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.Model.FinancialModel
{
    public class Portfolio
    {
        public double Value { get; private set; }
        public double[] Deltas { get; set; }
        public double FreeRiskDelta { get; set; }

        public Portfolio(double v, double[] d, double r)
        {
            Value = v;
            Deltas = d;
            FreeRiskDelta = r;
        }

        public Portfolio(Portfolio p)
        {
            Value = p.Value;
            Deltas = p.Deltas;
            FreeRiskDelta = p.FreeRiskDelta;
        }

        public void updateValue(double[] spot)
        {
            Value = scalaire(Deltas, spot) + FreeRiskDelta * RiskFreeRateProvider.GetRiskFreeRateAccruedValue(1.0 / 365);
        }

        public void updateFreeRiskDelta(double[] spot)
        {
            FreeRiskDelta = Value - scalaire(Deltas, spot);
        }

        private double scalaire(double[] a, double[] b)
        {
            var res = 0.0;
            for (var i = 0; i < a.Length; i++)
            {
                res += a[i] * b[i];
            }
            return res;
        }
    }
}
