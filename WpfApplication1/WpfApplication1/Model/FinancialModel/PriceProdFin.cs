using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.Model.FinancialModel
{
    public class PriceProdFin
    {
        public PriceProdFin(DateTime d, double p)
        {
            Date = d;
            Price = p; //Price of the financial product
        }
        public DateTime Date { get; set; }

        public double Price { get; set; }
    }
}
