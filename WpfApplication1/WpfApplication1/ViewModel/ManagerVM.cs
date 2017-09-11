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
        private int sampleNumber;
        private ChartValues<double> optp;
        private ChartValues<double> pfp;
        private ChartValues<double> trackingError;
        private string[] labels;
        private IDataFeedProvider marketSimulator;
        private FinancialComputation option;
        private double valPayOff;
        private double valPortfolio;


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
        public int Step { get; set; }
        public double ValPayOff { get; set; }
        public double ValPortfolio { get; set; }
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
        public ManagerVM(DateTime theDate, string estmWindow, string frequency, IDataFeedProvider simulator, FinancialComputation opt)
        {
            StartDate = theDate;
            SampleNumber = Int32.Parse(estmWindow);
            Step = Int32.Parse(frequency);
            Labels = GetDateSet(new List<DateTime>());
            marketSimulator = simulator;
            option = opt;

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
            foreach (DateTime it in MarketDataDates)
                allDates.Add(it.ToShortDateString());

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
        public void PleaseUpdateManager(DateTime theDate, string estmWindow, string frequency, IDataFeedProvider simulator, FinancialComputation opt)
        {
            StartDate = theDate;
            SampleNumber = Int32.Parse(estmWindow);
            Step = Int32.Parse(frequency);
            marketSimulator = simulator;
            option = opt;
            
            var window = 20;
            var res = option.GenChartData(window, StartDate, Step, marketSimulator);
            ValPayOff = option.PayOff;
            ValPortfolio = res.PortfolioValue.Last().Value;
            Labels = GetDateSet(option.MarketDataDates);
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
