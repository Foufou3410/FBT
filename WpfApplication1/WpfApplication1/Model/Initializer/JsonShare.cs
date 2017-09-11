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

        public Share ToShare()
        {
            return new Share(Name, Id);
        }
    }
}
