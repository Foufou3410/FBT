using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model.FinancialModel
{
    public class PriceAndDelta
    {
        public PriceAndDelta(DateTime d, double p, double[] del)
        {
            Date = d;
            Price = p; //Price of the financial product
            Deltas = del; //deltas of the sous-jacents in the optimal portfolio
        }
        public DateTime Date { get; set; }

        public double Price { get; set; }

        public double[] Deltas { get; set; }
    }
}
