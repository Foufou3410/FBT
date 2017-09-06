using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model.FinancialModel
{
    class PricePortfolio
    {
        public PricePortfolio(DateTime d, double p, double s, double r)
        {
            Date = d;
            Price = p;
            valShare = s;
            valSansRisque = r;
        }
        public DateTime Date { get; set; }

        public double Price { get; set; }

        public double valShare { get; set; }

        public double valSansRisque { get; set; }
    }
}
