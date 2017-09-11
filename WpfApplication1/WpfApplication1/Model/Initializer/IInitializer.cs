using FBT.Model.FinancialModel;
using System;
using System.Collections.Generic;

namespace FBT.Model.Initializer
{
    public interface IInitializer
    {
        List<FinancialComputation> initAvailableOptions(string namefile);
    }
}
