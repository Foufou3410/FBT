using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FBT.Model.Initializer
{
    class ParseTextFile
    {
        public Parameters Parsing(string file)
        {

            string[] lines = System.IO.File.ReadAllLines(file);
            var ListParam = new Dictionary<string, string>();

            foreach (string line in lines)
            {

                string[] words = Regex.Split(line, " : ");
                ListParam.Add(words[0], words[1]);
            }

            if (ListParam.ContainsKey("Maturity") && ListParam.ContainsKey("Strike") && ListParam.ContainsKey("Share Name(s)") && ListParam.ContainsKey("Option Type") && ListParam.ContainsKey("Estimation window") && ListParam.ContainsKey("Timespan of the simulation") && ListParam.ContainsKey("Rebalancing step"))
            {
                var maturityS = "";
                var strikeS = "";
                var shareName = "";
                var optionType = "";
                var windowS = "";
                var timespanS = "";
                var rebalStepS = "";

                ListParam.TryGetValue("Maturity", out maturityS);
                DateTime maturity = DateTime.Parse(maturityS);

                ListParam.TryGetValue("Strike", out strikeS);
                int strike = int.Parse(strikeS);

                ListParam.TryGetValue("Share Name(s)", out shareName);
                string[] shareNames = Regex.Split(shareName, ", ");

                ListParam.TryGetValue("Option Type", out optionType);

                ListParam.TryGetValue("Estimation window", out windowS);
                int EstimWindow = int.Parse(windowS);

                ListParam.TryGetValue("Timespan of the simulation", out timespanS);
                int timespan = int.Parse(timespanS);

                ListParam.TryGetValue("Rebalancing step", out rebalStepS);
                int rebalStep = int.Parse(rebalStepS);

                var listParams = new Parameters(maturity, strike, shareNames, optionType, EstimWindow, timespan, rebalStep);
                return listParams;
            }
            else
            {
                throw new Exception("Il manque des paramètres dans le fichier de configuration");
            }


        }


    }
}
