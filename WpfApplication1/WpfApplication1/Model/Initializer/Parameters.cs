using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.Model.Initializer
{
    class Parameters
    {

        public DateTime maturity { get; }
        public int strike { get; }
        public string[] shareNames { get; } 
        public string optionType { get; }
        public int window { get; }
        public int timespan { get; }
        public int rebalStep { get; }

        public Parameters(DateTime m, int s, string[] sn, string op, int w, int t, int r)
        {
            maturity = m;
            strike = s;
            shareNames = sn;
            optionType = op;
            window = w;
            timespan = t;
            rebalStep = r;
        }

    }
}
