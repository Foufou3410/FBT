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
    class ParseTextFile
    {
        /*public List<FinancialComputation> Parsing(string[] files)
        {

            var ListOptions = new List<FinancialComputation>();

            for (var k = 0; k < files.Count(); k++)
            {

                var file = files[k];




                string[] lines = System.IO.File.ReadAllLines(file);
                var ListParam = new Dictionary<string, string>();

                foreach (string line in lines)
                {

                    string[] words = Regex.Split(line, " : ");
                    ListParam.Add(words[0], words[1]);
                }

                if (ListParam.ContainsKey("Share Weight(s)") && ListParam.ContainsKey("Option Name") && ListParam.ContainsKey("Share Id(s)") && ListParam.ContainsKey("Maturity") && ListParam.ContainsKey("Strike") && ListParam.ContainsKey("Share Name(s)") && ListParam.ContainsKey("Option Type") && ListParam.ContainsKey("Estimation window") && ListParam.ContainsKey("Timespan of the simulation") && ListParam.ContainsKey("Rebalancing step") && ListParam.ContainsKey("Beginning of simulation"))
                {
                    var maturityS = "";
                    var startSimS = "";
                    var strikeS = "";
                    var shareName = "";
                    var shareId = "";
                    var optionType = "";
                    var optionName = "";
                    var windowS = "";
                    var timespanS = "";
                    var rebalStepS = "";
                    var shareWeightS = "";

                    ListParam.TryGetValue("Maturity", out maturityS);
                    DateTime maturity = DateTime.Parse(maturityS);

                    ListParam.TryGetValue("Beginning of simulation", out maturityS);
                    DateTime startSim = DateTime.Parse(startSimS);

                    ListParam.TryGetValue("Strike", out strikeS);
                    int strike = int.Parse(strikeS);

                    ListParam.TryGetValue("Share Name(s)", out shareName);
                    string[] shareNames = Regex.Split(shareName, ", ");

                    ListParam.TryGetValue("Share Weight(s)", out shareWeightS);
                    string[] shareWeight2 = Regex.Split(shareName, ", ");
                    double[] shareWeight = null;
                    for (var j = 0; j < shareWeight2.Count(); j++)
                    {
                        shareWeight[j] = double.Parse(shareWeight2[j]);
                    }

                    ListParam.TryGetValue("Share Id(s)", out shareId);
                    string[] shareIds = Regex.Split(shareId, ", ");

                    ListParam.TryGetValue("Option Type", out optionType);

                    ListParam.TryGetValue("Estimation window", out windowS);
                    int EstimWindow = int.Parse(windowS);

                    ListParam.TryGetValue("Timespan of the simulation", out timespanS);
                    int timespan = int.Parse(timespanS);

                    ListParam.TryGetValue("Rebalancing step", out rebalStepS);
                    int rebalStep = int.Parse(rebalStepS);

                    ListParam.TryGetValue("Option Name", out optionName);

                    var shareList = new List<Share>();
                    for (var i = 0; i < shareIds.Count(); i++)
                    {
                        shareList.Add(new Share(shareNames[i], shareIds[i]));
                    }

                    if (optionType == "Vanilla")
                    {
                        var opt = new VanillaCall(optionName, shareList[0], maturity, strike);
                        ListOptions.Add(new VanillaComputation(opt));
                    }
                    else
                    {
                        var opt = new BasketOption("basketBNP", shareList.ToArray(), shareWeight, maturity, strike);
                        ListOptions.Add(new BasketComputation(opt));
                    }

                }
                else
                {
                    throw new Exception("Il manque des paramètres dans le fichier de configuration");
                }
            }

            return ListOptions;
        }*/

        private readonly static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Error
        };
        public List<FinancialComputation> Parse()
        {

            var objects = new List<IOption>()
            {
                //new VanillaCall("vanillaBNP", new Share("BNP", "AC FP"), new DateTime(2018, 9, 6), 8),
                new JsonBasket("basketTest", new string[] { "AI FP", "BN FP", "CAP FP" }, new double[] { 0.3, 0.3, 0.4 }, new DateTime(2013, 6, 11), 9, new string[] { "AI FP", "BN FP", "CAP FP" })

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
