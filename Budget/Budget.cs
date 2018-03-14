using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Budget
    {

        public string YearMonth { get; set; }
        public int Amount { get; set; }

        private DateTime FirstDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
            }
        }

        private DateTime LastDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + TotalDays, "yyyyMMdd", null);
            }
        }

        private int TotalDays
        {
            get
            {
                return DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            }
        }

        private int DailyAmount
        {
            get
            {
                return Amount / TotalDays; 
            }
        }

        public int EffectiveAmount(Period period)
        {
            return DailyAmount * period.OverlappingDays(new Period(FirstDay, LastDay));
        }
    }
}
