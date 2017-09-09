using System;
using System.Collections.Generic;
using PricingLibrary.FinancialProducts;
using AppelWRE;
using FBT.Model.FinancialModel;

namespace FBT.Model.Initializer
{
    public class HardCodeInitializer : IInitializer
    {
        #region Public Properties
        public DateTime SimuStartDate { get; }

        public int SimuTimeSpan { get; }

        public int RebalancingStep { get; }

        public int EstimationWindow { get; }
        #endregion Public Properties

        #region Public Constructor
        public HardCodeInitializer()
        {
            SimuStartDate = new DateTime(2017, 9, 6);
            SimuTimeSpan = 365;
            RebalancingStep = 1;
            EstimationWindow = 50;
        }
        #endregion Public Constructor

        #region Public Methods
        public VanillaComputation initVanillaOpt()
        {
            var sousJacent = new Share("BNP", "AC FP");
            var date = SimuStartDate.AddDays(SimuTimeSpan);
            var opt = new VanillaCall("optionBNP", sousJacent, date, 8);
            return new VanillaComputation(opt);
        }

        public BasketComputation initBasketOpt()
        {
            var sousJacent1 = new Share("BNP", "1");
            var sousJacent2 = new Share("AXA", "2");
            var sousJacent3 = new Share("Accenture", "3");
            var date = SimuStartDate.AddDays(SimuTimeSpan);
            var opt = new BasketOption("basketBNP", new Share[]{sousJacent1, sousJacent2, sousJacent3}, new double[] { 0.5, 0.3, 0.2 }, date, 8);
            return new BasketComputation(opt);
        }
        #endregion Public Methods
    }
}
