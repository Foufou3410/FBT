using FBT.Model.FinancialModel;
using System.Collections.Generic;

namespace FBT.Model.Initializer
{
    public interface IInitializer
    {
        List<FinancialComputation> InitAvailableOptions(string namefile);
    }
}
