using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using WpfApplication1.Model.FinancialModel;
using WpfApplication1.Model.Initializer;

namespace Wpf.CartesianChart.PointShapeLine
{
    internal class MainWindowViewModel
    {
        #region Private Fields
        
        private HardCodeInitializer init;
        private FinancialComputation computer;
        private DateTime start;
        private uint timeLapse;
        private uint step;

        private MyDataFeed dataFeedProvider;

        #endregion Private Fields


        // To send away
        enum period : uint { year = 365, semester = 183, month = 30, week = 5, day = 1 };

        public MainWindowViewModel()
        {
            //to be refactored
            init = new HardCodeInitializer();
            computer = new FinancialComputation();

            start = new DateTime(2017, 9, 6);
            timeLapse = (uint)period.year;
            step = 2 * (uint)period.day;
            dataFeedProvider = new MyDataFeed();

            var opt = init.initOptionsUnivers(start, timeLapse- 1);
            var spot = dataFeedProvider.GenerateDataFeed(start, timeLapse- 1, opt);

            var riskFreeRate = init.initRiskFreeRate(step);

            var dates = init.getDatesOfSimuData(start, timeLapse- 1, step);
            var vol = init.getVolatilityOfSimuData(timeLapse- 1, step);

            var res = computer.computeDeltasAndPrice(dates, opt, spot, vol, step);
            var port = computer.computePricePortfolio(dates, res, spot, riskFreeRate, step);


            // Dialogue with team to match model output
            ChartValues<double> optp = new ChartValues<double>();
            ChartValues<double> pfp = new ChartValues<double>();
            ChartValues<double> trackingError = new ChartValues<double>();
            for (int i=0; i < res.Count; i++)
            {
                optp.Insert(i, res[i].Price);
                pfp.Insert(i, port[i].Price);
                trackingError.Insert(i, port[i].Price - res[i].Price);
            }
            

            // 1st chart - Option price + Portfolio value
            SeriesCollection = new SeriesCollection
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
            SeriesCollectionBis = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Tracking error",
                    Values = trackingError
                }
            };
            Labels = new[] { "Monday", "Tuesday", "Wenesday", "Thursday", "Frieday"};
            YFormatter = value => value.ToString("C");

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
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection SeriesCollectionBis { get; private set; }
    }
}