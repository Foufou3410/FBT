using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;

namespace FBT.Model.FinancialModel
{
    public class HistDataFeedProvider : IDataFeedProvider
    {
        string IDataFeedProvider.Name { get { return "HistoricalData"; } }


        int IDataFeedProvider.NumberOfDaysPerYear { get { return 365; } }


        List<DataFeed> IDataFeedProvider.GetDataFeed(IOption option, DateTime debDate)
        {
            using (DataClasses1DataContext asdc = new DataClasses1DataContext())
            {
                var res = new List<DataFeed>();
                var request = (from lignes in asdc.HistoricalShareValues
                               where (option.UnderlyingShareIds.Contains(lignes.id)) && (lignes.date.Date < option.Maturity.Date) && (lignes.date.Date > debDate.Date)
                               select (double)lignes.value);
                foreach (double r in request)
                {
                    Spots.Add(r);
                }
                return res;
            }
        }

        DateTime IDataFeedProvider.GetMinDate()
        {
            throw new NotImplementedException();
        }
    }
}