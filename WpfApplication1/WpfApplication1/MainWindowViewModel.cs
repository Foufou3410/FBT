using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;

namespace Wpf.CartesianChart.PointShapeLine
{
    internal class MainWindowViewModel
    {
        #region Private Fields
        
        private HardCodeInitializer init;
        private DateTime start;
        private int timeLapse;
        private int step;

        #endregion Private Fields


        // To send away
        enum period : int { year = 365, semester = 183, month = 30, week = 5, day = 1 };

        public MainWindowViewModel()
        {
            //to be refactored
            /*init = new HardCodeInitializer();
            

            start = new DateTime(2017, 9, 6);
            timeLapse = (int)period.year;
            step = 2 * (int)period.day;
            var window = 30; //to be binded
  
            var opt = init.initVanillaOpt(start, timeLapse- 1);
            var vanillaOpt = new VanillaComputation(opt, start, window);

            var riskFreeRate = init.initRiskFreeRate(step);
            var startRebalancing = start.AddDays(window);
            var dates = init.getRebalancingDates(startRebalancing, timeLapse - window - 1, step);

            var res = vanillaOpt.computePrice(dates);
            var port = vanillaOpt.computeValuePortfolio(dates, dates, riskFreeRate);



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
            YFormatter = value => value.ToString("C");*/

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