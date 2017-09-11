using System;
using System.Collections.Generic;
using PricingLibrary.FinancialProducts;
using AppelWRE;
using FBT.Model.FinancialModel;

namespace FBT.Model.Initializer
{
    public class HardCodeInitializer : IInitializer
    {
        #region Public Methods
        public List<FinancialComputation> initAvailableOptions()
        {
            var res = new List<FinancialComputation>();

            var sousJacentVanilla = new Share("AC", "AC FP     ");
            var maturityVanilla = new DateTime(2013, 9, 6);
            var vanilla = new VanillaCall("vanilla AC", sousJacentVanilla, maturityVanilla, 8);
            res.Add(new VanillaComputation(vanilla));

            var sousJacentBasket1 = new Share("AC", "AC FP     ");
            var sousJacentBasket2 = new Share("ACA", "ACA FP    ");
            var sousJacentBasket3 = new Share("EDF", "EDF FP    ");
            var maturityBasket = new DateTime(2013, 9, 6);
            var basket = new BasketOption("basket AC ACA EDF", new Share[] { sousJacentBasket1, sousJacentBasket2, sousJacentBasket3 }, new double[] { 0.5, 0.3, 0.2 }, maturityBasket, 8);
            res.Add(new BasketComputation(basket));

            return res;
        }
        #endregion Public Methods
    }
}
