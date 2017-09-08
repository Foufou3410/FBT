using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FBT.Model
{
    class Pattern
    {
        private Regex positiveDecimal;

        public Pattern()
        {
            positiveDecimal = new Regex(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$");
        }

        public Regex PositiveDecimal => positiveDecimal;
    }
}
