using System;
using System.Collections.Generic;
using Prism.Commands;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Threading;
using System.Windows.Controls;
using FBT.Model.Enum;
using System.Text.RegularExpressions;
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

        private bool enableRun;
        private string frequency = "0";
        private string estmWindow = "0";
        private double viewPayOff;
        
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

            var init = new HardCodeInitializer();
            optionList = init.initAvailableOptions();
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
            dispatcherTimer.Stop();
        }

        private void Calculate()
        {
            if(enableRun)
                dispatcherTimer.Stop();
            else
                dispatcherTimer.Start();
            enableRun = !enableRun;
        }

        private bool CanRun()
        {
            return(pattern.PositiveDecimal.IsMatch(Frequency) && pattern.PositiveInteger.IsMatch(EstimWindow) && ValuesType != null);
        }
        #endregion Handler

        #region Public Accessors

        public DelegateCommand CalculateCmd { get; private set; }
        public DatePicker DateBox { get; private set; }
        public DateTime TheDate { get; set;}
       
        public double ViewPort
        {
            get { return valPort; }
            set { SetProperty(ref valPort, value); }
        }

        public SeriesCollection PfOpChart { get; set; }
        

        public List<IDataFeedProvider> ValuesType { get { return viewTypesList; } }
        public IDataFeedProvider SelectedValuesType { get; set; }

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

        public string[] Labels { get; set; }
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


