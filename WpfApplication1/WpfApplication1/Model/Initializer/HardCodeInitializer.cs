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

            var sousJacentVanilla0 = new Share("AC", "AC FP     ");
            var maturityVanilla0 = new DateTime(2014, 9, 25);
            var vanilla0 = new VanillaCall("simu vanilla AC", sousJacentVanilla0, maturityVanilla0, 25);
            res.Add(new VanillaComputation(vanilla0));

            var sousJacentVanilla1 = new Share("AC", "AC FP     ");
            var maturityVanilla1 = new DateTime(2011, 9, 25);
            var vanilla1 = new VanillaCall("vanilla AC", sousJacentVanilla1, maturityVanilla1, 25);
            res.Add(new VanillaComputation(vanilla1));

            var sousJacentVanilla2 = new Share("BN", "BN FP     ");
            var maturityVanilla2 = new DateTime(2011, 9, 6);
            var vanilla2 = new VanillaCall("vanilla BN", sousJacentVanilla2, maturityVanilla2, 45);
            res.Add(new VanillaComputation(vanilla2));

            var sousJacentVanilla3 = new Share("CAP", "CAP FP    ");
            var maturityVanilla3 = new DateTime(2011, 9, 6);
            var vanilla3 = new VanillaCall("vanilla CAP", sousJacentVanilla3, maturityVanilla3, 35);
            res.Add(new VanillaComputation(vanilla3));

            var sousJacentBasket1 = new Share("AC", "AI FP     ");
            var sousJacentBasket2 = new Share("ACA", "CAP FP    ");
            var sousJacentBasket3 = new Share("EDF", "BN FP     ");
            var maturityBasket = new DateTime(2013, 6, 11);
            var basket = new BasketOption("basket AI CAP BN", new Share[] { sousJacentBasket1, sousJacentBasket2, sousJacentBasket3 }, new double[] { 0.3, 0.3, 0.4 }, maturityBasket, 9);
            res.Add(new BasketComputation(basket));

            return res;
        }
        #endregion Public Methods
    }
}
