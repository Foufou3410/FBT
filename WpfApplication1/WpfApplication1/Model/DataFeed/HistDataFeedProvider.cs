using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;

namespace FBT.DateFeed.FinancialModel
{
    public class HistDataFeedProvider : IDataFeedProvider
    {
        string IDataFeedProvider.Name { get { return "HistoricalData"; } }

        int IDataFeedProvider.NumberOfDaysPerYear { get { return 365; } }

        List<DataFeed> IDataFeedProvider.GetDataFeed(IOption option, DateTime debDate)
        {
            using (DataClasses1DataContext dataAccess = new DataClasses1DataContext())
            {
                IEnumerable<ShareValue> valueList = (from lignes in dataAccess.HistoricalShareValues
                                                     where (option.UnderlyingShareIds.Contains(lignes.id)) && (lignes.date.Date < option.Maturity.Date) && (lignes.date.Date > debDate.Date)
                                                     select new ShareValue(lignes.id, lignes.date, lignes.value));
                var dataFeedEnumerable = valueList.GroupBy(d => d.DateOfPrice,
                                         t => new { Symb = t.Id, Val = t.Value },
                                         (key, g) => new DataFeed(key, g.ToDictionary(e => e.Symb, e => e.Val)));

                return dataFeedEnumerable.ToList();
            }
        }

        DateTime IDataFeedProvider.GetMinDate()
        {
            using (DataClasses1DataContext dataAccess = new DataClasses1DataContext())
            {
                IEnumerable<DateTime> valueList = (from lignes in dataAccess.HistoricalShareValues
                                                   select lignes.date);
                return valueList.Min();
            }
        }
    }
}