using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;

namespace FBT.Model.Initializer
{
    class JsonShare
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public JsonShare(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public Share toShare()
        {
            return new Share(Name, Id);
        }
    }
}
