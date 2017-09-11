using System;
using System.Collections.Generic;
using System.Linq;
using PricingLibrary.FinancialProducts;
using FBT;

namespace PricingLibrary.Utilities.MarketDataFeed
{
    public class HistDataFeedProvider : IDataFeedProvider
    {
        public string Name { get { return "Historical Data"; } }

        public int NumberOfDaysPerYear { get { return 365; } }

        public List<DataFeed> GetDataFeed(IOption option, DateTime debDate)
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

        public DateTime GetMinDate()
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