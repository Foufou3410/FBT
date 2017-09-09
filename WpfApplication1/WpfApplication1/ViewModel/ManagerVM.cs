﻿using FBT.Model.Enum;
using FBT.Model.FinancialModel;
using FBT.Model.Initializer;
using LiveCharts;
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
        #endregion Private Attributs

        #region Public Accessor

        public ChartValues<double> Optp { get => optp; set => optp = value; }
        public ChartValues<double> Pfp { get => pfp; set => pfp = value; }
        public ChartValues<double> TrackingError { get => trackingError; set => trackingError = value; }
        public string[] Labels { get => labels; set => labels = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public int SampleNumber { get => sampleNumber; set => sampleNumber = value; }
        public int Step { get => step; set => step = value; }

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
            Labels = getDateSet();
            optp = new ChartValues<double>();
            pfp = new ChartValues<double>();
            trackingError = new ChartValues<double>();
        }

        // 
        // Abstract:
        //      Built the set of dates that will be printed to the xaxis
        //
        // Parameters:
        //      None - it's using object's element only.
        private string[] getDateSet()
        {
            List<string> allDates = new List<string>();
            for(DateTime date = StartDate; date <= StartDate.AddDays(SampleNumber); date = date.AddDays(Step))
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
        public void pleaseUpdateManager(DateTime theDate, string estmWindow, string frequency)
        {
            StartDate = theDate;
            SampleNumber = Int32.Parse(estmWindow);
            Step = Int32.Parse(frequency);

            var opt = init.initVanillaOpt(StartDate, SampleNumber - 1);
            var vanillaOpt = new VanillaComputation(opt, StartDate);
            var riskFreeRate = init.initRiskFreeRate(Step);
            var dates = init.getRebalancingDates(StartDate, SampleNumber - 1, Step);
 
            var share = vanillaOpt.computePrice(dates);
            var portefolio = vanillaOpt.computeValuePortfolio(dates, dates, riskFreeRate);

            for (int i = 0; i < share.Count; i++)
            {
                optp.Insert(i, share[i].Price);
                pfp.Insert(i, portefolio[i].Price);
                trackingError.Insert(i, portefolio[i].Price - share[i].Price);
            }
        }

        #endregion Public Constructor
    }
}
