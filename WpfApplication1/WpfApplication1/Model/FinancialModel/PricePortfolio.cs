using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model.FinancialModel
{
    class PricePortfolio
    {
        public PricePortfolio(DateTime d, double p)
        {
            Date = d;
            Price = p;
        }
        public DateTime Date { get; set; }

        public double Price { get; set; }
    }
}
