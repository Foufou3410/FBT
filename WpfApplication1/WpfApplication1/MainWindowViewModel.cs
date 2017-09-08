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

namespace FBT
{
    class MainWindowViewModel
    {
        #region Private Fields
        private Pattern pattern;
        private HardCodeInitializer init;
        private DateTime start;
        private int timeLapse;
        private int step;

        private List<String> viewTypesList;
        private DispatcherTimer dispatcherTimer;
        private double valPort;
        private double valPayOff;
        private bool enableRun;
        private string frequency = "1.0";
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

            valPort = 15.5;
            valPayOff = 5.2;

            TheDate = DateTime.Today;

            #endregion Public Buttons


            #region Public ToSendAway
            // REplaced by one call obj def


            // @TODO rename HARDCORE
            init = new HardCodeInitializer();

            start = TheDate;
            timeLapse = (int)Period.year;
            step = 2 * (int)Period.day;

            var opt = init.initVanillaOpt(start, timeLapse - 1);
            var vanillaOpt = new VanillaComputation(opt, start);
            var riskFreeRate = init.initRiskFreeRate(step);
            var dates = init.getRebalancingDates(start, timeLapse - 1, step);

            var share = vanillaOpt.computePrice(dates);
            var portefolio = vanillaOpt.computeValuePortfolio(dates, dates, riskFreeRate);

            // Dialogue with team to match model output
            ChartValues<double> optp = new ChartValues<double>();
            ChartValues<double> pfp = new ChartValues<double>();
            ChartValues<double> trackingError = new ChartValues<double>();
            for (int i = 0; i < share.Count; i++)
            {
                optp.Insert(i, share[i].Price);
                pfp.Insert(i, portefolio[i].Price);
                trackingError.Insert(i, portefolio[i].Price - share[i].Price);
            }
            #endregion Public ToSendAway

            // 1st chart - Option price + Portfolio value
            PfOpChart = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Option price",
                    Values = optp,
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Portfolio value",
                    Values = pfp,
                    PointGeometry = null
                }
            };
            // 2nd chart - tracking error
            TerrorChart = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Tracking error",
                    Values = trackingError
                }
            };
            Labels = new[] { start.ToShortDateString() };
            YFormatter = value => value.ToString("C");

            #region Private MightBeUseful
            // Modifying the series collection will animate and update the chart
            /*SeriesCollection.Add(new LineSeries
              {
                  Title = "Series 4",
                  Values = new ChartValues<double> { 5, 3, 2, 4 },
                  LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                  PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                  PointGeometrySize = 50,
                  PointForeground = Brushes.Gray
              });
            */

            // Modifying any series values will also animate and update the chart
            /* SeriesCollection[3].Values.Add(5d);*/
            #endregion Private MightBeUseful
        }

        #endregion Public Constructors

        public void SelectionVerification(object sender, EventArgs e)
        {
            EnableRun = false;
            dispatcherTimer.Stop();
        }
        

        public DelegateCommand CalculateCmd
        {
            get;
            private set;
        }

        private void Calculate()
        {
            dispatcherTimer.Start();
        }

        private bool CanRun()
        {
            return(pattern.PositiveDecimal.IsMatch(Frequency) && EstimWindow > 0 && ValuesType!=null);
        }
        
        public DatePicker DateBox { get; private set; }
        public DateTime TheDate { get; set; }
        public int EstimWindow { get; set; }
        
        public double ViewPort { get => valPort; }
        public List<string> ValuesType { get => viewTypesList; }
        public double ViewPayOff { get => valPayOff; }
        public SeriesCollection PfOpChart { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection TerrorChart { get; private set; }
        public string Frequency
        {
            get => frequency ;
            set
            {
                frequency = value;
                CalculateCmd.RaiseCanExecuteChanged();
            }
        }

        public bool EnableRun { get => enableRun; set => enableRun = value; }
    }

}


