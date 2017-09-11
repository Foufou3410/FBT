using FBT.Model.FinancialModel;
using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace FBT.Model.Initializer
{
    class ParseTextFileInitializer : IInitializer
    {
        private readonly static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Error
        };
        public List<FinancialComputation> initAvailableOptions()
        {

            var objects = new List<IOption>()
            {
                //new VanillaCall("vanillaBNP", new Share("BNP", "AC FP"), new DateTime(2018, 9, 6), 8),
                new JsonBasket("basket AI BN CAP", new string[] { "AI FP     ", "BN FP     ", "CAP FP    " }, new double[] { 0.3, 0.3, 0.4 }, new DateTime(2013, 6, 11), 45, new string[] { "AI FP", "BN FP", "CAP FP" })

                //new JsonBasket("basketBNP", new string[] { "1", "2", "3" }, new double[] { 0.5, 0.3, 0.2 }, new DateTime(2018, 9, 6), 8, new string[] {"BNP", "AXA", "ACCOR" })            
            };

            File.WriteAllText(@"test.json", JsonConvert.SerializeObject(objects, settings));


            var serialized = File.ReadAllText(@"test.json");
            var results = JsonConvert.DeserializeObject<List<IOption>>(serialized, settings);

            var finalRes = new List<FinancialComputation>();
            foreach (var res in results)
            {
                if (res.GetType() == typeof(FBT.Model.Initializer.JsonBasket))
                {
                    JsonBasket jres = (JsonBasket)res;
                    var bask = jres.ToBasket();
                    finalRes.Add(new BasketComputation(bask));
                }
                else
                {
                    finalRes.Add(new VanillaComputation((VanillaCall)res));
                }
            }



            return finalRes;
        }
    }
}
