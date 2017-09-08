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
        #endregion Private Attributs

        #region Public Constructor
        public ManagerVM(DateTime theDate, int estmWindow, string frequency)
        {
            init = new HardCodeInitializer();

            startDate = theDate;
            sampleNumber = (int)Period.month;
            step = 2 * (int)Period.day; // frequency

            optp = new ChartValues<double>();
            pfp = new ChartValues<double>();
            trackingError = new ChartValues<double>();
        }

        public ChartValues<double> Optp { get => optp; set => optp = value; }
        public ChartValues<double> Pfp { get => pfp; set => pfp = value; }
        public ChartValues<double> TrackingError { get => trackingError; set => trackingError = value; }

        public void pleaseUpdateManager()
        {
            var opt = init.initVanillaOpt(startDate, sampleNumber - 1);
            var vanillaOpt = new VanillaComputation(opt, startDate);
            var riskFreeRate = init.initRiskFreeRate(step);
            var dates = init.getRebalancingDates(startDate, sampleNumber - 1, step);

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
