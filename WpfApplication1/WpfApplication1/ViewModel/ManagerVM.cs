using FBT.Model.Enum;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;
using LiveCharts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.ViewModel
{
    public class ManagerVM
    {
        #region Private Attributs
        private HardCodeInitializer init;
        private DateTime startDate;
        private int sampleNumber;
        private int step;
        private ChartValues<double> optp;
        private ChartValues<double> pfp;
        private ChartValues<double> trackingError;
        private string[] labels;
        private SimulatedDataFeedProvider marketSimulator;

        #endregion Private Attributs

        #region Public Accessor

        public ChartValues<double> Optp
        {
            get { return optp; } 
            set { optp = value; }
        }
        public ChartValues<double> Pfp
        {
            get { return pfp; }
            set { pfp = value; }
        }
        public ChartValues<double> TrackingError {
            get { return trackingError; }
            set { trackingError = value; }
        }
        public string[] Labels { get; set; }
        public DateTime StartDate { get; set; }
        public int SampleNumber
        {
            get { return sampleNumber; }
            set { sampleNumber = value; }
        }
        public int Step
        {
            get { return step; }
            set { step = value; }
        }

        #endregion region Public Accessor

        #region Public Constructor

        //
        // Abstract:
        //     Creates a new instance of FBT.ViewModel.ManagerVM with the date to start estimation
        //     with a set estimation window and a specific frequency of reshuffle to invoke Model
        //
        // Parameters:
        //   theDate:
        //     The starting date to invoke modeling.
        //
        //   estmWindow:
        //     The estimated window - number of days - where the model is going to be ran
        //
        //   frequency:
        //     The frequency of reshuffle the portefolio
        public ManagerVM(DateTime theDate, string estmWindow, string frequency)
        {
            init = new HardCodeInitializer();


            StartDate = theDate;
            SampleNumber = Int32.Parse(estmWindow);
            Step = Int32.Parse(frequency);
            Labels = GetDateSet(new List<DateTime>());
            marketSimulator = new SimulatedDataFeedProvider();
            optp = new ChartValues<double>();
            pfp = new ChartValues<double>();
            trackingError = new ChartValues<double>();
        }
        #endregion Public Constructor

        // 
        // Abstract:
        //      Built the set of dates that will be printed to the xaxis
        //
        // Parameters:
        //      None - it's using object's element only.
        public string[] GetDateSet(List<DateTime> MarketDataDates)
        {
            List<string> allDates = new List<string>();
            for(DateTime date = StartDate; date <= StartDate.AddDays(SampleNumber); date = date.AddDays(1))
                allDates.Add(date.ToShortDateString());
            
            return (allDates.ToArray());
        }


        //
        // Abstract:
        //      Update managerVM's attributs.
        //
        // Parameters:
        //  theDate:
        //      DateTime where the modeling should be ran.
        //
        //  estWindow:
        //      String containing the period where the model should be ran.
        //
        //  frequency:
        //      String containing the step where portefolio is reshuffled.
        public void PleaseUpdateManager(DateTime theDate, string estmWindow, string frequency)
        {
            StartDate = theDate;
            SampleNumber = Int32.Parse(estmWindow);
            Step = Int32.Parse(frequency);
            
            var window = 20;
            var vanillaOpt = init.initAvailableOptions()[0];
            Labels = GetDateSet(vanillaOpt.MarketDataDates);
            Console.WriteLine(Labels.First());

            var res = vanillaOpt.GenChartData(window, StartDate, Step, marketSimulator);
            optp.Clear();
            pfp.Clear();
            trackingError.Clear();
            for (int i = 0; i < res.OptionPrice.Count; i++)
            {
                optp.Insert(i, res.OptionPrice[i]);
                pfp.Insert(i, res.PortfolioValue[i].Value);
                trackingError.Insert(i, res.OptionPrice[i] - res.PortfolioValue[i].Value);
            }
        }

        
    }
}
