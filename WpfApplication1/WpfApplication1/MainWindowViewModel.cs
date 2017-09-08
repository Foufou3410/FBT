using System;
using System.Collections.Generic;
using AppelWRE;
using System.Windows;
using Prism.Commands;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using Prism.Mvvm;


namespace FBT
{
    class MainWindowViewModel
    {
        #region Private Fields

        private HardCodeInitializer init;
        private DateTime start;
        private int timeLapse;
        private int step;
        private List<String> viewTypesList;
        private bool tickerRun;
        private DispatcherTimer dispatcherTimer;
        private double valPort;
        private double valPayOff;
        


        #endregion Private Fields

        // To send away
        enum period : int { year = 365, semester = 183, month = 30, week = 5, day = 1 };
        #region Public Constructors

        public MainWindowViewModel()
        {

            #region Public Buttons

            CalculateCmd = new DelegateCommand(RunTicker, CanRunTicker);
            
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(SelectionVerification); 
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            viewTypesList = new List<string>();
            viewTypesList.Add("Historique");
            viewTypesList.Add("Simulées");

            valPort = 15.5;
            valPayOff = 5.2;

            theDate = DateTime.Today;

            #endregion Public Buttons


            #region Public ToSendAway
            // REplaced by one call obj def

            
            // @TODO rename HARDCORE
            init = new HardCodeInitializer();
         
            start = theDate;
            timeLapse = (int)period.year;
            step = 2 * (int)period.day;

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
            tickerRun = false;
            String message = "oki";
            
            if (selectedValuesType == null)
            {
                message="Veuillez renseigner le type de données à selectionner";
                System.Windows.Forms.MessageBox.Show(message, "Erreur");
            }
            else if (EstimWindow<= 0)
            {
                message = "Fenetre d'estimation invalide";
                System.Windows.Forms.MessageBox.Show(message, "Erreur");
            }
            else if(Frequency<=0)
            {
                message ="Frequence invalide";
                System.Windows.Forms.MessageBox.Show(message, "Erreur");
           }
            else
                System.Windows.Forms.MessageBox.Show("ok", "Erreur");
            dispatcherTimer.Stop();
            TickerRun = false;
        }

        public List<string> ValuesType
        {
            get
            {
                return viewTypesList;
            }
        }

        public string selectedValuesType { get; set; }
        
        public DelegateCommand CalculateCmd { get; private set; }

        private void RunTicker()
        {
            dispatcherTimer.Start();
            TickerRun = true;
        }

        private bool CanRunTicker()
        {
            return !TickerRun;
        }
       
        public bool TickerRun
        {
            get { return tickerRun; }
            set
            {
                CalculateCmd.RaiseCanExecuteChanged();
            }
        }

        public DatePicker dateBox { get; private set; }
        public DateTime theDate { get; set; }
        public int EstimWindow { get;  set; }
        public int Frequency { get; set; }
        public double ViewPort
        {
            get
            {
                return valPort;
            }
        }

        public double ViewPayOff
        {
            get
            {
                return valPayOff;
            }
        }

        public SeriesCollection PfOpChart { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection TerrorChart { get; private set; }

    }

}


