using System.Text.RegularExpressions;

namespace FBT.Model
{
    class Pattern
    {
        private Regex positiveDecimal;
        private Regex positiveInteger;

        public Pattern()
        {
            positiveDecimal = new Regex(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$");
            positiveInteger = new Regex(@"^[1-9]\d*$");
        }

        public Regex PositiveDecimal => positiveDecimal;

        public Regex PositiveInteger => positiveInteger;
    }
}
