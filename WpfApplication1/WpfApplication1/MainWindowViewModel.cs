using System;
using System.Collections.Generic;
using Prism.Commands;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Threading;
using FBT.Model;
using FBT.ViewModel;
using PricingLibrary.Utilities.MarketDataFeed;
using Prism.Mvvm;

namespace FBT
{
    public class MainWindowViewModel :BindableBase
    {
        #region Private Fields
        private ManagerVM vp;
        private Pattern pattern;
        private DispatcherTimer dispatcherTimer;
        private double valPort;
        private List<IDataFeedProvider> viewTypesList;
        private List<FinancialComputation> optionList;
        private IDataFeedProvider selectedValuesType;
        private bool enableRun;
        private string frequency = "1";
        private string estmWindow = "50";
        private double viewPayOff;
        private string[] labels;
        private DateTime theDate;
        #endregion Private Fields

        #region Public Constructors
        public MainWindowViewModel()
        { 
            pattern = new Pattern();
            EnableRun = false;
            
            #region Public Buttons
            CalculateCmd = new DelegateCommand(Calculate, CanRun);
            
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(SelectionVerification);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            viewTypesList = new List<IDataFeedProvider>() {new SimulatedDataFeedProvider(), new HistDataFeedProvider()};
            SelectedValuesType = viewTypesList[0];

            //var init = new HardCodeInitializer();
            var init = new ParseTextFileInitializer();
            init.GenerateJson(@"test2.json");

            optionList = init.InitAvailableOptions(@"test2.json");
            SelectedOption = optionList[0];

            #endregion Public Buttons

            vp = new ManagerVM(TheDate, EstimWindow, Frequency, SelectedValuesType, SelectedOption);

            #region Charts Initialization
            // 1st chart - Option price + Portfolio value
            PfOpChart = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Option price",
                    Values = vp.Optp,
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Portfolio value",
                    Values = vp.Pfp,
                    PointGeometry = null
                }
            };
            // 2nd chart - Tracking error
            TerrorChart = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Tracking error",
                    Values = vp.TrackingError
                }
            };

            Labels = vp.Labels;
            YFormatter = value => value.ToString("C");
            #endregion Charts Initialization     
        }
        #endregion Public Constructors

        #region Handler
        public void SelectionVerification(object sender, EventArgs e)
        {
            vp.PleaseUpdateManager(TheDate, EstimWindow, Frequency, SelectedValuesType, SelectedOption);
            ViewPayOff = vp.ValPayOff;
            ViewPort = vp.ValPortfolio;
            Labels = vp.Labels;
            dispatcherTimer.Stop();
        }

        private void Calculate()
        {
            dispatcherTimer.Start();
        }

        private bool CanRun()
        {
            return (
                pattern.PositiveInteger.IsMatch(Frequency) &&
                pattern.PositiveInteger.IsMatch(EstimWindow)
                );
        }
        #endregion Handler

        #region Public Accessors
        public DelegateCommand CalculateCmd { get; private set; }

        public DateTime TheDate
        {
            get { return theDate; }
            set { SetProperty(ref theDate, value); }

        }
       
        public double ViewPort
        {
            get { return valPort; }
            set { SetProperty(ref valPort, value); }
        }

        public SeriesCollection PfOpChart { get; set; }
        

        public List<IDataFeedProvider> ValuesType { get { return viewTypesList; } }

        public IDataFeedProvider SelectedValuesType
        {
            get { return selectedValuesType; }
            set
            {
                TheDate = value.GetMinDate();
                SetProperty(ref selectedValuesType, value);
            }
        }

        public List<FinancialComputation> OptionList { get { return optionList; } }

        public FinancialComputation SelectedOption { get; set; }

        public string EstimWindow {
            get { return estmWindow; }
            set
            {
                estmWindow = value;
                CalculateCmd.RaiseCanExecuteChanged();
            }
        }

        public string Frequency
        {
            get { return frequency; }
            set
            {
                frequency = value;
                CalculateCmd.RaiseCanExecuteChanged();
            }
        }

        public bool EnableRun
        {
            get { return enableRun; }
            set { enableRun = value; }
        }

        public string[] Labels
        {
            get { return labels; }
            set { SetProperty(ref labels, value); }
        }

        public Func<double, string> YFormatter { get; set; }

        public SeriesCollection TerrorChart { get; private set; }

        public double ViewPayOff { get { return viewPayOff; }
            set
            {
                SetProperty(ref viewPayOff, value);
            } 
        }
        #endregion Public Accessors
    }

}


