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
using Prism.Mvvm;

namespace FBT
{
    public class MainWindowViewModel :BindableBase
    {
        #region Private Fields

        private ManagerVM vp;

        private Pattern pattern;
        private DispatcherTimer dispatcherTimer;
        private List<String> viewTypesList;
        private bool enableRun;
        private string frequency = "1";
        private string estmWindow = "50";
        private double viewPayOff;
        private double valPort;
        private string[] labels;
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
            
            viewTypesList = new List<string>();
            viewTypesList.Add("Historique");
            viewTypesList.Add("Simulées");

            #endregion Public Buttons

            vp = new ManagerVM(TheDate, EstimWindow, Frequency);

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
        }

        #endregion Public Constructors

        #region Handler
        public void SelectionVerification(object sender, EventArgs e)
        {
            Console.WriteLine(TheDate.ToString());
            vp.PleaseUpdateManager(TheDate, EstimWindow, Frequency);
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
        

        public List<string> ValuesType { get { return viewTypesList; } }
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


