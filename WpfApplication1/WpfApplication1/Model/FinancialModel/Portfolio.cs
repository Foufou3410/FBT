using PricingLibrary.Utilities.MarketDataFeed;

namespace FBT.Model.FinancialModel
{
    public class Portfolio
    {
        #region Public Properties
        public double Value { get; private set; }
        public double[] Deltas { get; set; }
        public double FreeRiskDelta { get; set; }
        #endregion Public Properties

        #region Public Constructor
        public Portfolio(double v, double[] d, double[] initialSpots)
        {
            Value = v;
            Deltas = d;
            FreeRiskDelta = v - Scalaire(d, initialSpots);
        }

        public Portfolio(Portfolio p)
        {
            Value = p.Value;
            Deltas = p.Deltas;
            FreeRiskDelta = p.FreeRiskDelta;
        }
        #endregion Public Properties

        #region Public Methods
        public void UpdateValue(double[] spot)
        {
            Value = Scalaire(Deltas, spot) + FreeRiskDelta * RiskFreeRateProvider.GetRiskFreeRateAccruedValue(1.0 / 365);
        }

        public void UpdateFreeRiskDelta(double[] spot)
        {
            FreeRiskDelta = Value - Scalaire(Deltas, spot);
        }
        #endregion Public Methods

        #region Private Methods
        private double Scalaire(double[] a, double[] b)
        {
            var res = 0.0;
            for (var i = 0; i < a.Length; i++)
            {
                res += a[i] * b[i];
            }
            return res;
        }
        #endregion Private Methods
    }
}
