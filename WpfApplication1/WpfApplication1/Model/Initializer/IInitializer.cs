using FBT.Model.FinancialModel;
using System;

namespace FBT.Model.Initializer
{
    public interface IInitializer
    {

        VanillaComputation initVanillaOpt();

        BasketComputation initBasketOpt();
    }
}
