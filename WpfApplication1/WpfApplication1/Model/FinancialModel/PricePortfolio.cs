﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.Model.FinancialModel
{
    public class PricePortfolio
    {
        public PricePortfolio(DateTime d, double p, List<double> s, double r)
        {
            Date = d;
            Price = p;
            valShares = s;
            valSansRisque = r;
        }
        public DateTime Date { get; set; }

        public double Price { get; set; }

        public List<double> valShares { get; set; }

        public double valSansRisque { get; set; }
    }
}